﻿using Hartsy.Extensions.MagicPromptExtension.WebAPI.Models;
using SwarmUI.Utils;
using System.Text.Json;

namespace Hartsy.Extensions.MagicPromptExtension
{
    public static class BackendSchema
    {
        /// <summary>Get the schema type for the backend.</summary>
        /// <param name="type"></param>
        /// <param name="inputText"></param>
        /// <param name="model"></param>
        /// <returns>Returns an object with the schema type for the backend.</returns>
        public static object GetSchemaType(string type, string inputText, string model)
        {
            type = type.ToLower();
            switch (type)
            {
                case "ollama":
                    return OllamaRequestBody(inputText, model);
                case "openai":
                case "openaiapi":
                case "openrouter":
                    return OpenAICompatibleRequestBody(inputText, model);
                case "anthropic":
                    return AnthropicRequestBody(inputText, model);
                default:
                    Logs.Error("Unsupported or null backend. Check the config.json");
                    return null;
            }
        }

        /// <summary> </summary>
        /// <param name="inputText"></param>
        /// <param name="currentModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object OllamaRequestBody(string inputText, string currentModel)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(currentModel))
            {
                throw new ArgumentException("Input text or model cannot be null or empty.");
            }
            return new
            {
                model = currentModel,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = inputText
                    }
                },
                stream = false
            };
        }

        /// <summary>Generates a request body for OpenAI and similar backends (Oogabooga).</summary>
        /// <param name="inputText"></param>
        /// <param name="currentModel"></param>
        /// <returns></returns>
        public static object OpenAICompatibleRequestBody(string inputText, string currentModel)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(currentModel))
            {
                throw new ArgumentException("Input text or model cannot be null or empty.");
            }
            return new
            {
                model = currentModel,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = inputText
                    }
                },
                temperature = 1.0,
                max_tokens = 200,
                top_p = 0.9,
                stream = false
            };
        }

        /// <summary>Generates a request body for the Claude (Anthropic API) based on the input text and model.</summary>
        /// <param name="inputText">The text input provided by the user.</param>
        /// <param name="currentModel">The Claude model being used (e.g., "claude-3-5-sonnet-20241022").</param>
        /// <returns>An anonymous object representing the request body for the Claude API.</returns>
        /// <exception cref="ArgumentException">Thrown when the input text or model is null or empty.</exception>
        public static object AnthropicRequestBody(string inputText, string currentModel)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(currentModel))
            {
                throw new ArgumentException("Input text or model cannot be null or empty.");
            }

            return new
            {
                model = currentModel,
                max_tokens = 1024,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = inputText
                    }
                }
            };
        }
    }
}

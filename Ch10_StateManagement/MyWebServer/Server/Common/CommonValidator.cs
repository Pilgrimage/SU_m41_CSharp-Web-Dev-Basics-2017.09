﻿namespace MyWebServer.Server.Common
{
    using System;

    public static class CommonValidator
    {
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"{name} can't be null or empty.");
            }
        }

    }
}
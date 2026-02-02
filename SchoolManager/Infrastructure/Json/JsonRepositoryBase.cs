using SchoolManager.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Json
{
    public abstract class JsonRepositoryBase<T>
    {
        private void EnsureFileExists()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }
        protected string _filePath = "";
        protected JsonRepositoryBase(string filePath)
        {
            _filePath = filePath;
            EnsureFileExists();
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };

        protected List<T> Load()
        {
            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions)
                   ?? new List<T>();
        }

        protected void Save(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}
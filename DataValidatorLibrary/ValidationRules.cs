using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidatorLibrary
{
    // Правило проверки на null
    public class NotNullRule<T> : IValidationRule<T>
    {
        public void Validate(T data)
        {
            if (data == null)
            {
                throw new ValidationException("Data cannot be null.");
            }
        }
    }

    // Правило проверки длины строки
    public class StringLengthRule : IValidationRule<string>
    {
        private readonly int _min;
        private readonly int _max;

        public StringLengthRule(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public void Validate(string data)
        {
            // Добавляем проверку на null
            if (data == null)
            {
                throw new ValidationException("Data cannot be null.");
            }

            // Проверяем длину строки
            if (data.Length < _min || data.Length > _max)
            {
                throw new ValidationException($"String length must be between {_min} and {_max} characters.");
            }
        }

    }

    // Правило проверки положительности числа
    public class PositiveNumberRule : IValidationRule<int>
    {
        public void Validate(int data)
        {
            if (data <= 0)
            {
                throw new ValidationException("Number must be positive.");
            }
        }
    }
}

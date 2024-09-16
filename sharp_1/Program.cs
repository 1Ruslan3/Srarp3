using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Program
{

    public class Tokenizer : IEnumerable<string>, IDisposable
    {
        private readonly HashSet<char> delimiters;
        private readonly string filePath;
        private StreamReader reader;
        private bool disposed = false;

        public Tokenizer(HashSet<char> delimiters, string filePath)
        {
            this.delimiters = delimiters ?? throw new ArgumentNullException(nameof(delimiters));
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath); 
            }

            this.reader = new StreamReader(filePath);
        }

        public IEnumerator<string> GetEnumerator()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Tokenizer), "Object has been disposed.");
            }

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                foreach (string token in TokenizeLine(line))
                {
                    yield return token;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<string> TokenizeLine(string line)
        {
            int start = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (delimiters.Contains(line[i]))
                {
                    if (i > start)
                    {
                        yield return line.Substring(start, i - start);
                    }
                    start = i + 1;
                }
            }

            if (start < line.Length)
            {
                yield return line.Substring(start);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (reader != null)
                    {
                        reader.Dispose();
                        reader = null;
                    }
                }

                // Dispose unmanaged resources
                disposed = true;
            }
        }

        ~Tokenizer()
        {
            Dispose(false);
        }
    }

    static void Main()
    {
        var delimiters = new HashSet<char> { ' ', ',', '.', ':', ';', '\n', '\t' };

        string filePath = "example.txt";

        using (var tokenizer = new Tokenizer(delimiters, filePath))
        {
            foreach (string token in tokenizer)
            {
                Console.WriteLine(token);
            }
        }
    }
}
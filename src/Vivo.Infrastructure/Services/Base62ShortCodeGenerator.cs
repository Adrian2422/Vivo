namespace Vivo.Infrastructure.services;

using System.Security.Cryptography;
using Application.Interfaces;

public class Base62ShortCodeGenerator: IShortCodeGenerator
{
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const int CodeLength = 7;

    public string Generate()
    {
        var buffer = new char[CodeLength];
        for (var i = 0; i < CodeLength; i++)
            buffer[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        return new string(buffer);
    }
}
using System;
using System.Text;

public class HillCipher
{
    private static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    private static int[][] MatrixMultiply(int[][] matrix1, int[][] matrix2, int mod)
    {
        int rows = matrix1.Length;
        int cols = matrix2[0].Length;
        int[][] result = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                for (int k = 0; k < matrix2.Length; k++)
                {
                    result[i][j] += Mod(matrix1[i][k] * matrix2[k][j], mod);
                    result[i][j] %= mod;
                }
            }
        }

        return result;
    }

    private static int[][] InverseMatrix(int[][] matrix, int mod)
    {
        int n = matrix.Length;
        int[][] augmentedMatrix = new int[n][];

        for (int i = 0; i < n; i++)
        {
            augmentedMatrix[i] = new int[2 * n];
            for (int j = 0; j < n; j++)
            {
                augmentedMatrix[i][j] = matrix[i][j];
            }
            augmentedMatrix[i][i + n] = 1;
        }

        for (int i = 0; i < n; i++)
        {
            int pivot = augmentedMatrix[i][i];
            if (pivot == 0)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (augmentedMatrix[j][i] != 0)
                    {
                        int[] temp = augmentedMatrix[i];
                        augmentedMatrix[i] = augmentedMatrix[j];
                        augmentedMatrix[j] = temp;
                        break;
                    }
                }
                pivot = augmentedMatrix[i][i];
            }

            int pivotInverse = Mod(1, mod);
            for (int j = 0; j < 2 * n; j++)
            {
                augmentedMatrix[i][j] = Mod(augmentedMatrix[i][j] * pivotInverse, mod);
            }

            for (int j = 0; j < n; j++)
            {
                if (j != i)
                {
                    int factor = augmentedMatrix[j][i];
                    for (int k = 0; k < 2 * n; k++)
                    {
                        augmentedMatrix[j][k] = Mod(augmentedMatrix[j][k] - factor * augmentedMatrix[i][k], mod);
                    }
                }
            }
        }

        int[][] inverseMatrix = new int[n][];
        for (int i = 0; i < n; i++)
        {
            inverseMatrix[i] = new int[n];
            for (int j = 0; j < n; j++)
            {
                inverseMatrix[i][j] = augmentedMatrix[i][j + n];
            }
        }

        return inverseMatrix;
    }
public static string Encrypt(string plaintext, int[][] keyMatrix, int mod)
{

    int n = keyMatrix.Length;
    int[] paddedPlaintext = new int[n];
    StringBuilder ciphertext = new StringBuilder();

    for (int i = 0; i < plaintext.Length; i++)
    {
        paddedPlaintext[i % n] = plaintext[i] - 'A'; // Assume que o texto em claro consiste apenas de letras maiúsculas do alfabeto
        if (i % n == n - 1)
        {
            int[] encryptedBlock = new int[n];
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    encryptedBlock[j] += keyMatrix[j][k] * paddedPlaintext[k];
                    encryptedBlock[j] %= mod;
                }
                ciphertext.Append((char)(encryptedBlock[j] + 'A')); // Assume que o texto cifrado será em letras maiúsculas do alfabeto
            }
        }
    }

    if (plaintext.Length % n != 0)
    {
        int[] encryptedBlock = new int[n];
        for (int j = 0; j < n; j++)
        {
            for (int k = 0; k < n; k++)
            {
                encryptedBlock[j] += keyMatrix[j][k] * paddedPlaintext[k];
                encryptedBlock[j] %= mod;
            }
            ciphertext.Append((char)(encryptedBlock[j] + 'A')); // Assume que o texto cifrado será em letras maiúsculas do alfabeto
        }
    }

    return ciphertext.ToString();
}

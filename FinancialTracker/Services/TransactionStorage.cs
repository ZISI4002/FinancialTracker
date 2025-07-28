using FinancialTracker.Models;
using FinancialTracker.Services.FinancialTracker.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FinancialTracker.Services
{
    internal static class TransactionStorage
    {
        // Path to the application data folder (e.g., %AppData%\FinancialTracker)
        private static readonly string _fileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinancialTracker");

        // Path to the main encrypted file
        private static readonly string _filePath = Path.Combine(_fileDirectory, "transactions.dat");

        // Cache for loaded transactions (to avoid reading and decrypting every time)
        private static List<TransactionDataModel> _cachedTransactions;

        /// <summary>
        /// Saves the transaction list as encrypted JSON.
        /// Also creates a hidden backup file.
        /// </summary>
        public static void Save(List<TransactionDataModel> transactions)
        {
            // Create directory if it doesn't exist
            if (!Directory.Exists(_fileDirectory))
                Directory.CreateDirectory(_fileDirectory);

            // Serialize transactions to JSON
            string json = JsonConvert.SerializeObject(transactions, Newtonsoft.Json.Formatting.Indented);

            // Encrypt JSON data
            byte[] encryptedData = EncryptionService.Encrypt(json);

            // Save the main file
            File.WriteAllBytes(_filePath, encryptedData);

            // Create a backup file
            string backupPath = _filePath + ".bak";
            File.WriteAllBytes(backupPath, encryptedData);

            // Make the backup file hidden
            File.SetAttributes(backupPath, FileAttributes.Hidden);

            // Clear cache after saving (so next load will read from file)
            _cachedTransactions = null;
        }

        /// <summary>
        /// Loads the transaction list from the encrypted file.
        /// If the main file is missing, tries to restore from backup.
        /// If nothing is found, returns an empty list.
        /// </summary>
        public static List<TransactionDataModel> Load()
        {
            // Return cached data if available
            if (_cachedTransactions != null)
                return _cachedTransactions;

            // Check if the main file exists
            if (File.Exists(_filePath))
            {
                byte[] encryptedData = File.ReadAllBytes(_filePath);
                string json = EncryptionService.Decrypt(encryptedData);

                // Deserialize JSON into a list of transactions
                _cachedTransactions = JsonConvert.DeserializeObject<List<TransactionDataModel>>(json) ?? new List<TransactionDataModel>();
                return _cachedTransactions;
            }

            // If main file is missing, check backup
            string backupPath = _filePath + ".bak";
            if (File.Exists(backupPath))
            {
                byte[] encryptedData = File.ReadAllBytes(backupPath);
                string json = EncryptionService.Decrypt(encryptedData);

                // Restore the main file from backup
                File.WriteAllBytes(_filePath, encryptedData);

                // Deserialize and cache
                _cachedTransactions = JsonConvert.DeserializeObject<List<TransactionDataModel>>(json) ?? new List<TransactionDataModel>();
                return _cachedTransactions;
            }

            // If nothing is found, return an empty list
            _cachedTransactions = new List<TransactionDataModel>();
            return _cachedTransactions;
        }
    }

}

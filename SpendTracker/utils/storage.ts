import AsyncStorage from "@react-native-async-storage/async-storage";

// Define the Transaction type here so we can reuse it across the app
export type Transaction = {
  id: string;
  type: "debit" | "credit";
  amount: number;
  merchant: string;
  date: string;
  raw?: string; // the original SMS/notification text
};

const STORAGE_KEY = "transactions";

// Save a new transaction
export async function saveTransaction(tx: Transaction): Promise<void> {
  try {
    const existing = await getTransactions();
    const updated = [tx, ...existing];
    await AsyncStorage.setItem(STORAGE_KEY, JSON.stringify(updated));
  } catch (error) {
    console.error("Error saving transaction:", error);
  }
}

// Get all transactions
export async function getTransactions(): Promise<Transaction[]> {
  try {
    const data = await AsyncStorage.getItem(STORAGE_KEY);
    return data ? JSON.parse(data) : [];
  } catch (error) {
    console.error("Error getting transactions:", error);
    return [];
  }
}

// Delete all transactions
export async function clearTransactions(): Promise<void> {
  try {
    await AsyncStorage.removeItem(STORAGE_KEY);
  } catch (error) {
    console.error("Error clearing transactions:", error);
  }
}

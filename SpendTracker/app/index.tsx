import {
  clearTransactions,
  getTransactions,
  saveTransaction,
  Transaction,
} from "@/utils/storage";
import { useEffect, useState } from "react";
import {
  AppState,
  FlatList,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from "react-native";

import { authenticate } from "@/utils/auth";
import LockScreen from "./lock";

import { parseTransaction } from "@/utils/parser";
import { SafeAreaView } from "react-native-safe-area-context";

export default function HomeScreen() {
  const [isLocked, setIsLocked] = useState(true);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [loading, setLoading] = useState(true);

  // ✅ ALL useEffects at the top, before any conditionals
  useEffect(() => {
    handleAuthentication();
  }, []);

  useEffect(() => {
    const subscription = AppState.addEventListener("change", (nextState) => {
      if (nextState === "background" || nextState === "inactive") {
        setIsLocked(true);
      }
    });
    return () => subscription.remove();
  }, []);

  // ✅ Functions defined after hooks
  async function handleAuthentication() {
    const success = await authenticate();
    if (success) {
      setIsLocked(false);
      loadTransactions();
    }
  }

  async function loadTransactions() {
    const saved = await getTransactions();
    setTransactions(saved);
    setLoading(false);
  }

  const balance = transactions.reduce((total, tx) => {
    return tx.type === "credit" ? total + tx.amount : total - tx.amount;
  }, 0);

  async function addTestTransaction() {
    const newTransaction: Transaction = {
      id: Date.now().toString(),
      type: Math.random() > 0.5 ? "credit" : "debit",
      amount: parseFloat((Math.random() * 100).toFixed(2)),
      merchant: "Test Merchant",
      date: new Date().toLocaleDateString(),
      raw: "Test notification text",
    };
    await saveTransaction(newTransaction);
    setTransactions((prev) => [newTransaction, ...prev]);
  }

  async function handleClear() {
    await clearTransactions();
    setTransactions([]);
  }

  // ✅ Conditionals AFTER all hooks and functions
  if (isLocked) {
    return (
      <LockScreen
        onUnlock={() => {
          setIsLocked(false);
          loadTransactions();
        }}
      />
    );
  }

  if (loading) {
    return (
      <View style={styles.container}>
        <Text style={styles.subtitle}>Loading...</Text>
      </View>
    );
  }

  async function addParsedTransaction() {
    const testMessages = [
      "Your account has been debited $52.30 at Woolworths on 07/04/2026",
      "Payment of $9.99 to Netflix was successful",
      "You have received $1,500.00 from Employer Pty Ltd",
      "Cashback of $5.00 credited to your account",
    ];

    // Pick a random test message
    const randomMessage =
      testMessages[Math.floor(Math.random() * testMessages.length)];
    const transaction = parseTransaction(randomMessage);

    if (transaction) {
      await saveTransaction(transaction);
      setTransactions((prev) => [transaction, ...prev]);
    }
  }

  return (
    <SafeAreaView style={styles.safeArea}>
      <View style={styles.container}>
        {/* Header */}
        <View style={styles.header}>
          <Text style={styles.title}>💰 SpendTracker</Text>
          <Text style={styles.balance}>${balance.toFixed(2)}</Text>
          <Text style={styles.subtitle}>Current Balance</Text>
        </View>

        {/* Transaction List */}
        <FlatList
          data={transactions}
          keyExtractor={(item) => item.id}
          style={styles.list}
          ListEmptyComponent={
            <Text style={styles.empty}>No transactions yet</Text>
          }
          renderItem={({ item }) => (
            <View style={styles.transaction}>
              <View>
                <Text style={styles.merchant}>{item.merchant}</Text>
                <Text style={styles.date}>{item.date}</Text>
              </View>
              <Text
                style={[
                  styles.amount,
                  { color: item.type === "credit" ? "#4ecca3" : "#ff6b6b" },
                ]}
              >
                {item.type === "credit" ? "+" : "-"}${item.amount}
              </Text>
            </View>
          )}
        />

        {/* Buttons */}
        <TouchableOpacity style={styles.button} onPress={addParsedTransaction}>
          <Text style={styles.buttonText}>+ Parse Test SMS</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.clearButton} onPress={handleClear}>
          <Text style={styles.clearButtonText}>Clear All</Text>
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#1a1a2e",
  },
  header: {
    alignItems: "center",
    paddingTop: 60,
    paddingBottom: 30,
    backgroundColor: "#16213e",
  },
  title: {
    fontSize: 22,
    fontWeight: "bold",
    color: "#ffffff",
    marginBottom: 16,
  },
  balance: {
    fontSize: 48,
    fontWeight: "bold",
    color: "#4ecca3",
  },
  subtitle: {
    fontSize: 14,
    color: "#888888",
    marginTop: 4,
  },
  list: {
    flex: 1,
    paddingHorizontal: 16,
    marginTop: 16,
  },
  empty: {
    textAlign: "center",
    color: "#888888",
    marginTop: 40,
    fontSize: 16,
  },
  transaction: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: "#16213e",
    padding: 16,
    borderRadius: 12,
    marginBottom: 10,
  },
  merchant: {
    color: "#ffffff",
    fontSize: 16,
    fontWeight: "600",
  },
  date: {
    color: "#888888",
    fontSize: 12,
    marginTop: 4,
  },
  amount: {
    fontSize: 18,
    fontWeight: "bold",
  },
  button: {
    backgroundColor: "#4ecca3",
    margin: 16,
    marginBottom: 8,
    padding: 16,
    borderRadius: 12,
    alignItems: "center",
  },
  buttonText: {
    color: "#1a1a2e",
    fontSize: 16,
    fontWeight: "bold",
  },
  clearButton: {
    margin: 16,
    marginTop: 0,
    padding: 16,
    borderRadius: 12,
    alignItems: "center",
    borderWidth: 1,
    borderColor: "#ff6b6b",
  },
  clearButtonText: {
    color: "#ff6b6b",
    fontSize: 16,
    fontWeight: "bold",
  },
  safeArea: {
    flex: 1,
    backgroundColor: "#16213e", // matches header color so top bar looks clean
  },
});

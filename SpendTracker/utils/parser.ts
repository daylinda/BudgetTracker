// utils/parser.ts

import { Transaction } from "./storage";

// Keywords that indicate money was spent
const DEBIT_KEYWORDS = [
  "debited",
  "spent",
  "paid",
  "payment",
  "purchased",
  "withdrawn",
  "deducted",
  "charged",
  "transferred out",
];

// Keywords that indicate money was received
const CREDIT_KEYWORDS = [
  "credited",
  "received",
  "added",
  "deposited",
  "refund",
  "cashback",
  "transfer in",
  "payment received",
];

// Common Australian banks & payment apps
const MERCHANT_PATTERNS = [
  /at ([A-Z][a-zA-Z\s&]+?)(?:\s+on|\s+for|\s+\d|\.)/, // "at Woolworths on"
  /from ([A-Z][a-zA-Z\s]+?)(?:\s+on|\s+for|\.|$)/, // "from John Smith"
  /to ([A-Z][a-zA-Z\s]+?)(?:\s+on|\s+for|\.|$)/, // "to Netflix"
];

export function parseTransaction(message: string): Transaction | null {
  const lower = message.toLowerCase();

  // Detect type
  const isDebit = DEBIT_KEYWORDS.some((k) => lower.includes(k));
  const isCredit = CREDIT_KEYWORDS.some((k) => lower.includes(k));

  // If neither debit nor credit keyword found, ignore
  if (!isDebit && !isCredit) return null;

  // Extract amount - handles $100, $1,234.56, AUD 50, 99.99
  const amountMatch = message.match(
    /(?:AUD|USD|A\$|\$)?\s?(\d{1,3}(?:,\d{3})*(?:\.\d{1,2})?)/,
  );
  if (!amountMatch) return null;

  const amount = parseFloat(amountMatch[1].replace(",", ""));
  if (isNaN(amount) || amount <= 0) return null;

  // Extract merchant name
  let merchant = "Unknown";
  for (const pattern of MERCHANT_PATTERNS) {
    const match = message.match(pattern);
    if (match) {
      merchant = match[1].trim();
      break;
    }
  }

  return {
    id: Date.now().toString(),
    type: isDebit ? "debit" : "credit",
    amount,
    merchant,
    date: new Date().toLocaleDateString(),
    raw: message,
  };
}

// Test the parser with sample messages
export function runParserTests(): void {
  const testMessages = [
    "Your account has been debited $52.30 at Woolworths on 07/04/2026",
    "Payment of $9.99 to Netflix was successful",
    "You have received $1,500.00 from Employer Pty Ltd",
    "Cashback of $5.00 credited to your account",
    "ATM withdrawal of $200.00 at Commonwealth Bank",
    "This is a random message with no transaction info",
  ];

  console.log("=== Parser Test Results ===");
  testMessages.forEach((msg) => {
    const result = parseTransaction(msg);
    console.log("\nMessage:", msg);
    console.log(
      "Result:",
      result ? JSON.stringify(result, null, 2) : "Not a transaction",
    );
  });
}

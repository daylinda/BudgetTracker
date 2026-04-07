import * as LocalAuthentication from "expo-local-authentication";

// Check if device supports biometrics
export async function isBiometricAvailable(): Promise<boolean> {
  const compatible = await LocalAuthentication.hasHardwareAsync();
  const enrolled = await LocalAuthentication.isEnrolledAsync();
  return compatible && enrolled;
}

// Prompt the user to authenticate
export async function authenticate(): Promise<boolean> {
  const biometricAvailable = await isBiometricAvailable();

  const result = await LocalAuthentication.authenticateAsync({
    promptMessage: "👆 Verify your identity to open SpendTracker",
    fallbackLabel: "Use PIN instead",
    disableDeviceFallback: false, // allows PIN/pattern fallback
  });

  return result.success;
}

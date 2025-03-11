# Keylogger Detector

🔍 **Keylogger Detector** is a simple application designed to detect potential keyloggers, suspicious processes, and abnormal network activity on a Windows machine. It continuously monitors system processes, keyboard activity, and network connections to identify any malicious behavior associated with keyloggers or surveillance software.

---

## 🛠️ Features

- **Process Monitoring**: Detects suspicious processes related to keyloggers or spyware by checking running processes for keywords like "keylog", "logger", "sniffer", and more.
- **Keylogging Behavior Detection**: Monitors keyboard activity, and if abnormal keystrokes (e.g., 50 key presses in 10 seconds) are detected, it flags this as potential keylogger behavior.
- **Network Activity Monitoring**: Tracks network activity for suspicious connections on ports commonly used by malicious software (e.g., ports 21, 23, 445, 3389) and checks for unusual remote IPs outside of trusted DNS services.

---

## 🚀 How to Use

1. **Clone the repository**:
    ```bash
    gh repo clone YaroslavDenysiuk23/KeyloggerDetector
    ```

2. **Build and Run**:
    - Open the project in Visual Studio or your preferred C# development environment.
    - Build the solution and run the application.
    - The console will display real-time alerts for any suspicious processes, abnormal keyboard activity, or potentially harmful network connections.

3. **Monitoring**:
    - The tool runs continuously, and any detected suspicious activity will be printed to the console.

---

## ⚠️ Sample Output

```bash
🔍 Keylogger Detector started...

⚠ Suspicious process detected: keylog
⚠ High keyboard activity detected! Potential keylogger.
⚠ Suspicious network connection detected: 192.168.1.100:8080

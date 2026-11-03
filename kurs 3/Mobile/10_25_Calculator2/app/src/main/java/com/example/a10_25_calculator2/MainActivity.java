package com.example.a10_25_calculator2;

import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.Locale;

public class MainActivity extends AppCompatActivity {

    /** Максимальное количество символов при вводе числа */
    private static final int MAX_INPUT_LENGTH = 15;

    private TextView tvDisplay;

    private String currentInput = "0";
    private double storedValue = 0;
    private String pendingOperator = null;
    private boolean startNewNumber = true;
    private boolean hasError = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        tvDisplay = findViewById(R.id.tvDisplay);
        bindButtons();
        refreshDisplay();
    }

    // ------------------------------------------------------------------ //
    //  Привязка кнопок
    // ------------------------------------------------------------------ //

    private void bindButtons() {
        // Все цифровые кнопки берут текст прямо из себя
        int[] digitIds = {
                R.id.btZero, R.id.btOne, R.id.btTwo, R.id.btThree, R.id.btFour,
                R.id.btFive, R.id.btSix, R.id.btSeven, R.id.btEight, R.id.btNine
        };
        for (int id : digitIds) {
            Button b = findViewById(id);
            b.setOnClickListener(v -> onDigit(b.getText().toString()));
        }

        findViewById(R.id.btComma).setOnClickListener(v -> onComma());
        findViewById(R.id.btAdd).setOnClickListener(v -> onOperator("+"));
        findViewById(R.id.btSubtract).setOnClickListener(v -> onOperator("-"));
        findViewById(R.id.btMultiply).setOnClickListener(v -> onOperator("x"));
        findViewById(R.id.btDivision).setOnClickListener(v -> onOperator("/"));
        findViewById(R.id.btEquals).setOnClickListener(v -> onEquals());
        findViewById(R.id.btPercent).setOnClickListener(v -> onPercent());
        findViewById(R.id.btSign).setOnClickListener(v -> onSign());
        findViewById(R.id.btClearChar).setOnClickListener(v -> onClearChar());
        findViewById(R.id.btClearAll).setOnClickListener(v -> resetAll());
    }

    // ------------------------------------------------------------------ //
    //  Обработчики
    // ------------------------------------------------------------------ //

    private void onDigit(String digit) {
        if (hasError) resetAll();

        if (startNewNumber) {
            currentInput = digit.equals("0") ? "0" : digit;
            startNewNumber = false;
        } else {
            if (currentInput.equals("0")) {
                if (!digit.equals("0")) currentInput = digit;
            } else if (currentInput.equals("-0")) {
                if (!digit.equals("0")) currentInput = "-" + digit;
            } else if (currentInput.length() < MAX_INPUT_LENGTH) {
                currentInput += digit;
            }
        }
        refreshDisplay();
    }

    private void onComma() {
        if (hasError) resetAll();

        if (startNewNumber) {
            currentInput = "0.";
            startNewNumber = false;
        } else if (!currentInput.contains(".")) {
            currentInput += ".";
        }
        refreshDisplay();
    }

    private void onOperator(String operator) {
        if (hasError) return;

        // Если уже есть операция и введено второе число — считаем промежуточный результат
        if (pendingOperator != null && !startNewNumber) {
            if (!computePending()) return;   // деление на 0 -> показана ошибка
        } else {
            storedValue = parse(currentInput);
        }

        pendingOperator = operator;
        startNewNumber = true;
        refreshDisplay();
    }

    private void onEquals() {
        if (hasError || pendingOperator == null) return;

        double second = startNewNumber ? storedValue : parse(currentInput);
        Double result = calculate(storedValue, second, pendingOperator);
        if (result == null) {
            showError();
            return;
        }

        storedValue = result;
        currentInput = format(result);
        pendingOperator = null;
        startNewNumber = true;
        refreshDisplay();
    }

    private void onPercent() {
        if (hasError) return;

        double value = parse(currentInput);
        if ("+".equals(pendingOperator) || "-".equals(pendingOperator)) {
            value = storedValue * value / 100.0;
        } else {
            value = value / 100.0;
        }

        currentInput = format(value);
        startNewNumber = false;
        refreshDisplay();
    }

    private void onSign() {
        if (hasError) resetAll();
        if (currentInput.equals("0") || currentInput.equals("0.")) return;

        if (currentInput.startsWith("-")) {
            currentInput = currentInput.substring(1);
        } else {
            currentInput = "-" + currentInput;
        }
        refreshDisplay();
    }

    private void onClearChar() {
        if (hasError) {
            resetAll();
            return;
        }
        if (startNewNumber && currentInput.equals('0')) {
            currentInput = "0";
            return;
        }

        currentInput = currentInput.substring(0, currentInput.length() - 1);
        if (currentInput.isEmpty() || currentInput.equals("-")) {
            currentInput = "0";
            startNewNumber = true;
        }
        refreshDisplay();
    }

    private void resetAll() {
        currentInput = "0";
        storedValue = 0;
        pendingOperator = null;
        startNewNumber = true;
        hasError = false;
        refreshDisplay();
    }

    //  Вычисления

    /** Выполняет промежуточное вычисление storedValue (op) currentInput. */
    private boolean computePending() {
        double second = parse(currentInput);
        Double result = calculate(storedValue, second, pendingOperator);
        if (result == null) {
            showError();
            return false;
        }
        storedValue = result;
        currentInput = format(result);
        return true;
    }

    /** @return результат или null при делении на ноль */
    private Double calculate(double a, double b, String op) {
        if (op == null) return b;
        switch (op) {
            case "+": return a + b;
            case "-": return a - b;
            case "x": return a * b;
            case "/":
                if (b == 0) return null;
                return a / b;
            default:  return b;
        }
    }

    //  Вспомогательные

    private double parse(String s) {
        try {
            return Double.parseDouble(s);
        } catch (NumberFormatException e) {
            return 0;
        }
    }

    private String format(double value) {
        if (Double.isNaN(value) || Double.isInfinite(value)) return "0";

        if (Math.abs(value) >= 1e15) {
            return String.format(Locale.US, "%.6E", value);
        }

        String s = String.format(Locale.US, "%.10f", value);
        if (s.contains(".")) {
            s = s.replaceAll("0+$", "");
            if (s.endsWith(".")) s = s.substring(0, s.length() - 1);
        }
        if (s.equals("-0") || s.isEmpty()) s = "0";
        return s;
    }

    private void refreshDisplay() {
        tvDisplay.setText(currentInput.replace('.', ','));
    }

    private void showError() {
        hasError = true;
        startNewNumber = true;
        pendingOperator = null;
        storedValue = 0;
        currentInput = "0";
        tvDisplay.setText("Error");
    }
}
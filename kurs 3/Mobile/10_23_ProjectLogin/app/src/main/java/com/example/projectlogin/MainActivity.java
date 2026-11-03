package com.example.projectlogin;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    EditText etName;
    EditText etPhone;
    EditText etBirthDay;
    TextView tvHello;

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

        etName = findViewById(R.id.etName);
        etPhone = findViewById(R.id.etPhone);
        etBirthDay = findViewById(R.id.etBirthDay);
        tvHello = findViewById(R.id.tvHello);
        Button button = findViewById(R.id.button);

        button.setOnClickListener(v -> great());
    }

    void great(){
        try {
            String name = etName.getText().toString().trim();
            int year = Integer.parseInt(etBirthDay.getText().toString().trim().substring(6));
            int phone = Integer.parseInt(etPhone.getText().toString().trim());
            tvHello.setText(getString(R.string.hello, name, 2026 - year));
        }
        catch (NumberFormatException e){
            Toast.makeText(this, R.string.error, Toast.LENGTH_LONG).show();
        }
    }

}
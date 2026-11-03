package com.example.a10_23_clicker;

import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "HelloApp";

    private int clickCount = 0;

    private EditText etName;
    private TextView tvGreeting;
    private TextView tvCounter;

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
        tvGreeting = findViewById(R.id.tvGreeting);
        tvCounter = findViewById(R.id.tvCounter);
        Button btnGreet = findViewById(R.id.btnGreet);
        Button btnCount = findViewById(R.id.btnCount);
        Switch swWarm = findViewById(R.id.swWarm);

        btnGreet.setOnClickListener(v -> greet());
        btnCount.setOnClickListener(v -> countClick());
        swWarm.setOnCheckedChangeListener((v, isChecked) -> changeBackgroud(v, isChecked));

        updateCounter();
        Log.d(TAG, "onCreate: приложение запущено");
    }

    void greet(){
        String name = etName.getText().toString().trim();
        if (name.isEmpty()){
            Toast.makeText(this, R.string.toast_empty_name, Toast.LENGTH_SHORT);
            return;
        }
        tvGreeting.setText(getString(R.string.greeting, name));
    }

    void countClick(){
        clickCount++;
        updateCounter();
    }

    void updateCounter(){
        tvCounter.setText(getString(R.string.counter_format, clickCount));
    }

    
}
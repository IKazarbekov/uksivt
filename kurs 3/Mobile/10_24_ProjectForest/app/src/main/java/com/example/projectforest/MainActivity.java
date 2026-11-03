package com.example.projectforest;

import android.graphics.Color;
import android.os.Bundle;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    int status = 0;
    boolean creating = false;
    LinearLayout layout;
    TextView tvWater;

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

        layout = findViewById(R.id.mainLayout);
        tvWater = findViewById(R.id.tvWater);
        Button bCreate = findViewById(R.id.bCreate);
        Button bKill = findViewById(R.id.bKill);
        Button bHello = findViewById(R.id.bHello);
        Button bWater1 = findViewById(R.id.bWater1);
        Button bWater2 = findViewById(R.id.bWater2);

        bCreate.setOnClickListener(v -> create());
        bWater1.setOnClickListener(v -> water1());
        bWater2.setOnClickListener(v -> water2());
        bKill.setOnClickListener(v -> kill());
        bHello.setOnClickListener(v -> hello());

    }

    void create(){
        if (creating == false)
        {
            creating = true;
            status = 0;
        }
        updateImage();
    }

    void hello(){
        String msg = "";
        if (!creating){
            msg = "Молчание";
        }
        else if (status < 80){
            msg = "ПОЛИВАЙ МЕНЯЯЯ";
        }
        else
            msg = "О НЕТ ОСТАНОВИСЬ!!";
        Toast.makeText(this, msg, Toast.LENGTH_SHORT).show();
    }

    void water1(){
        if (!creating) return;
        status++;
        updateImage();
    }

    void water2(){
        if (!creating) return;
        status += 10;
        updateImage();
    }

    void kill(){
        creating = false;
        updateImage();
    }

    void updateImage(){
        if (creating == false) {
            if (status > 0)
                layout.setBackgroundResource(R.mipmap.forest5);
            else
                layout.setBackgroundResource(R.mipmap.forest4);
        }
        else
            if (status > 100)
                Integer.parseInt("Hahaha");
            else if (status > 75)
                layout.setBackgroundResource(R.mipmap.forest3);
            else if (status > 50)
                layout.setBackgroundResource(R.mipmap.forest2);
            else if (status > 25)
                layout.setBackgroundResource(R.mipmap.forest1);
            else
                layout.setBackgroundResource(R.mipmap.forest0);
        tvWater.setText(getString(R.string.status, status));

        if (status >= 50)
            tvWater.setTextColor(Color.BLUE);
        else if (status >= 10)
            tvWater.setTextColor(Color.GREEN);
        else
            tvWater.setTextColor(Color.WHITE);
    }

}
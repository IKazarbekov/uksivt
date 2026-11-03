plugins {
    alias(libs.plugins.android.application)
}

    android {
    namespace = "com.example.a10_23_clicker"
        compileSdk {
            version = release(37)
        }

        defaultConfig {
      applicationId = "com.example.a10_23_clicker"
    minSdk = 23
    targetSdk = 37
    versionCode = 1
    versionName = "1.0"

      testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
          release {
              optimization {
                  enable = true
                  packageScope = setOf("androidx.**", "kotlin.**", "kotlinx.**")
              }
          }
      }
        compileOptions {
            sourceCompatibility = JavaVersion.VERSION_11
            targetCompatibility = JavaVersion.VERSION_11
        }
    }

  dependencies {
      implementation(libs.appcompat)
      testImplementation(libs.junit)
      androidTestImplementation(libs.espresso.core)
      androidTestImplementation(libs.ext.junit)
  }
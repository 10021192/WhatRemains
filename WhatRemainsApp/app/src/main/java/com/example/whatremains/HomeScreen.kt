package com.example.whatremains

import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import okhttp3.Call
import okhttp3.Callback
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import java.io.IOException

@Composable
fun CompanionHomeScreen(
    client: OkHttpClient,
    onPlayMiniGame: () -> Unit
) {
    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(
                brush = Brush.verticalGradient(
                    colors = listOf(
                        Color(0xFF2E3192),
                        Color(0xFF1BFFFF)
                    )
                )
            )
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(
                text = "What Remains",
                fontSize = 28.sp,
                fontWeight = FontWeight.Bold,
                color = Color.White,
                textAlign = TextAlign.Center
            )
            Spacer(modifier = Modifier.height(12.dp))

            Text(
                text = "Claim your reward or earn shop discounts\nwith a fun mini-game!",
                fontSize = 16.sp,
                color = Color.LightGray,
                textAlign = TextAlign.Center
            )
            Spacer(modifier = Modifier.height(24.dp))

            ClaimCoinsButton(client)

            Spacer(modifier = Modifier.height(24.dp))

            Button(
                onClick = { onPlayMiniGame() },
                colors = ButtonDefaults.buttonColors(
                    containerColor = Color(color = 0xFFFF9800)
                )
            ) {
                Text(text = "Play Tapping Mini-Game", fontSize = 18.sp, color = Color.White)
            }
        }
    }
}

@Composable
fun ClaimCoinsButton(client: OkHttpClient) {
    val context = LocalContext.current

    Button(
        onClick = {
            val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()
            val requestBody = "{}".toRequestBody(mediaType) // empty JSON

            val request = Request.Builder()
                .url("https://whatremains-server.vercel.app/hourly_rewards/claim/1") // local dev server
                .post(requestBody)
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    (context as? ComponentActivity)?.runOnUiThread {
                        Toast.makeText(context, "Failed: ${e.message}", Toast.LENGTH_SHORT).show()
                    }
                }
                override fun onResponse(call: Call, response: Response) {
                    val responseBody = response.body?.string() ?: ""
                    response.close()

                    if (response.isSuccessful) {
                        (context as? ComponentActivity)?.runOnUiThread {
                            Toast.makeText(context, "Response: $responseBody", Toast.LENGTH_LONG).show()
                        }
                    } else {
                        (context as? ComponentActivity)?.runOnUiThread {
                            Toast.makeText(
                                context,
                                "Error: ${response.code}\nBody: $responseBody",
                                Toast.LENGTH_LONG
                            ).show()
                        }
                    }
                }
            })
        },
        colors = ButtonDefaults.buttonColors(
            containerColor = Color(color = 0xFFFF9800)
        )
    ) {
        Text(text = "Claim Reward", fontSize = 20.sp, color = Color.White)
    }
}
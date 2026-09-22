package com.example.whatremains

import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.absoluteOffset
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import okhttp3.Call
import okhttp3.Callback
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import java.io.IOException

@Composable
fun PopUpCoinsMiniGameScreen(
    client: OkHttpClient,
    onReturn: () -> Unit
) {
    val context = LocalContext.current

    // Score + coin list
    var score by remember { mutableStateOf(0) }
    var coins by remember { mutableStateOf(listOf<PopCoin>()) }

    var timeLeft by remember { mutableStateOf(15) }
    var gameRunning by remember { mutableStateOf(true) }

    val coinWidthDp = 70.dp
    val coinHeightDp = 70.dp

    // "Bounding box" for each coin is ~70 dp
    val coinRadiusDp = 35.dp

    // A measure for spawn area
    val minX = 0.dp
    val maxX = 300.dp
    val minY = 50.dp
    val maxY = 500.dp

    // 1) Countdown Timer: reduce timeLeft each second until it hits 0
    LaunchedEffect(Unit) {
        while (timeLeft > 0) {
            delay(1000L)
            timeLeft--
        }
        // timeLeft is now 0 => stop the game
        gameRunning = false
        // Clear all coins so they can't be clicked
        coins = emptyList()
    }

    // 2) Spawn new coins every second while gameRunning
    LaunchedEffect(gameRunning) {
        while (gameRunning) {
            delay(300L)

            // Attempt to spawn a new coin at random position,
            // ensuring no overlap with existing ones
            val newCoin = trySpawnCoin(coins, minX, maxX, minY, maxY, coinRadiusDp)
            if (newCoin != null) {
                coins = coins + newCoin
            }
        }
    }

    // 3) Periodically remove coins after some lifetime (2 sec, e.g.)
    LaunchedEffect(coins) {
        while(true) {
            delay(200L)
            val now = System.currentTimeMillis()
            coins = coins.filter { (now - it.spawnTime) < it.lifetime }
            if (!gameRunning) break
        }
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(
                brush = Brush.verticalGradient(
                    colors = listOf(
                        Color(0xFF3B3B98), // top color
                        Color(0xFF28326E)  // bottom color
                    )
                )
            )
    ) {
        // Show timeLeft + score at top
        Column(modifier = Modifier
            .align(Alignment.TopStart)
            .padding(16.dp)) {
            Text(text = "Time Left: $timeLeft", color = Color.White)
            Text(text = "Score: $score", color = Color.White)
        }

        // Render coins
        coins.forEach { c ->
            Image(
                painter = painterResource(id = R.drawable.coin_gold),
                contentDescription = "Pop-up coin",
                modifier = Modifier
                    .absoluteOffset(x = c.x, y = c.y)
                    .size(width = coinWidthDp, height = coinHeightDp)
                    .clickable {
                        // user tapped coin => score++ + remove coin
                        score++
                        coins = coins - c
                    },
                contentScale = ContentScale.Fit
            )
        }

        if (!gameRunning) {
            // game ended => show a "Finish" to post the final score
            Button(
                onClick = {
                    postScoreToServer(client, score) {
                        (context as? ComponentActivity)?.runOnUiThread {
                            Toast.makeText(context, "Score posted!", Toast.LENGTH_SHORT).show()
                            onReturn()
                        }
                    }
                },
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF3949AB)),
                modifier = Modifier
                    .align(Alignment.BottomCenter)
                    .padding(16.dp)
            ) {
                Text("Finish & Send Score", color = Color.White)
            }
        } else {
            // game still running => optionally let user finish early
            Button(
                onClick = {
                    gameRunning = false
                },
                colors = ButtonDefaults.buttonColors(containerColor = Color.Gray),
                modifier = Modifier
                    .align(Alignment.BottomCenter)
                    .padding(16.dp)
            ) {
                Text("Stop Early", color = Color.White)
            }
        }
    }
}

private fun postScoreToServer(
    client: OkHttpClient,
    score: Int,
    onSuccess: () -> Unit
) {
    val jsonBody = """{"score": $score}""".toRequestBody("application/json; charset=utf-8".toMediaTypeOrNull())
    val request = Request.Builder()
        .url("https://whatremains-server.vercel.app/player/1/discount")
        .post(jsonBody)
        .build()

    client.newCall(request).enqueue(object : Callback {
        override fun onFailure(call: Call, e: IOException) { }
        override fun onResponse(call: Call, response: Response) {
            response.close()
            if (response.isSuccessful) {
                onSuccess()
            }
        }
    })
}
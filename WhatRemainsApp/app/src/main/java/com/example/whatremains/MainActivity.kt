package com.example.whatremains

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.runtime.Composable
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.example.whatremains.ui.theme.WhatRemainsTheme
import okhttp3.OkHttpClient
import kotlin.random.Random

class MainActivity : ComponentActivity() {
    private val client = OkHttpClient()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            WhatRemainsTheme {
                AppNavigation(client)
            }
        }
    }
}

fun trySpawnCoin(
    existingCoins: List<PopCoin>,
    minX: Dp,
    maxX: Dp,
    minY: Dp,
    maxY: Dp,
    coinRadius: Dp
): PopCoin? {
    val triesMax = 10
    repeat(triesMax) {
        val xCandidate = randomDp(minX, maxX)
        val yCandidate = randomDp(minY, maxY)

        // check overlap
        val noOverlap = existingCoins.none { coin ->
            val dx = (xCandidate.value + coinRadius.value) - (coin.x.value + coinRadius.value)
            val dy = (yCandidate.value + coinRadius.value) - (coin.y.value + coinRadius.value)
            val distSq = dx*dx + dy*dy
            val minDist = coinRadius.value * 2
            distSq < (minDist * minDist)
        }

        if (noOverlap) {
            return PopCoin(
                x = xCandidate,
                y = yCandidate,
                spawnTime = System.currentTimeMillis(),
                lifetime = 2000L
            )
        }
    }
    return null
}

fun randomDp(min: Dp, max: Dp): Dp {
    val minF = min.value
    val maxF = max.value
    // Generate a float in [0..1), scale to [0..(maxF - minF)], then add minF
    val rndVal = Random.nextFloat() * (maxF - minF) + minF
    return rndVal.dp
}

data class PopCoin(
    val x: Dp,
    val y: Dp,
    val spawnTime: Long,
    val lifetime: Long
)

@Preview(showBackground = true)
@Composable
fun PreviewHomeScreen() {
    WhatRemainsTheme {
        CompanionHomeScreen(
            client = OkHttpClient(),
            onPlayMiniGame = {}
        )
    }
}

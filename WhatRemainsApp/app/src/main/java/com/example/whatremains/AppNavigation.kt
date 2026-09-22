package com.example.whatremains

import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import okhttp3.OkHttpClient

@Composable
fun AppNavigation(client: OkHttpClient) {
    // If true, show the mini-game. Otherwise, show the home screen.
    var showMiniGame by remember { mutableStateOf(false) }

    if (showMiniGame) {
        // Show the mini-game UI
        PopUpCoinsMiniGameScreen(
            client = client,
            onReturn = { showMiniGame = false } // go back to home after finishing
        )
    } else {
        // Show the home companion screen
        CompanionHomeScreen(
            client = client,
            onPlayMiniGame = { showMiniGame = true }
        )
    }
}
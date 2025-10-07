<template>
  <div class="auth-page">
    <div class="auth-card">
      <div class="tabs">
        <button
          :class="{ active: mode === 'login' }"
          @click="mode = 'login'"
        >Вход</button>
        <button
          :class="{ active: mode === 'register' }"
          @click="mode = 'register'"
        >Регистрация</button>
      </div>

      <LoginForm v-if="mode === 'login'" />
      <RegisterForm v-else />
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import LoginForm from "../components/LoginForm.vue";
import RegisterForm from "../components/RegisterForm.vue";

const mode = ref("login");
</script>

<style scoped>
.auth-page {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
  background: #525252
}

.auth-card {
  background: #2b2b2b;
  padding: 2rem;
  border-radius: 16px;
  width: 380px;
  color: #eee;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
}

.tabs {
  display: flex;
  justify-content: space-around;
  margin-bottom: 1.5rem;
}

.tabs button {
  background: none;
  border: none;
  color: #bbb;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  cursor: pointer;
  border-bottom: 2px solid transparent;
  transition: 0.3s;
}

.tabs button.active {
  color: #fff;
  border-color: #800000;
}
</style>

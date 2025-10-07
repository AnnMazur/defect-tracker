<template>
    <form @submit.prevent="handleLogin">
      <div class="form-group">
        <label for="email">E-mail</label>
        <input v-model="email" type="email" required />
      </div>
  
      <div class="form-group">
        <label for="password">Пароль</label>
        <input v-model="password" type="password" required />
      </div>
  
      <button type="submit" class="auth-btn">Войти</button>
      <p v-if="error" class="error">{{ error }}</p>
    </form>
  </template>
  
  <script setup>
  import { ref } from "vue";
  import { useRouter } from "vue-router";
  import { useAuthStore } from "../store/auth";
  
  const email = ref("");
  const password = ref("");
  const error = ref("");
  const router = useRouter();
  const auth = useAuthStore();
  
  const handleLogin = async () => {
    try {
      await auth.login(email.value, password.value);
      router.push("/projects");
    } catch (err) {
      error.value =
        err.response?.data?.message || "Ошибка входа. Проверьте данные.";
    }
  };
  </script>
  
  <style scoped>
  .form-group {
    margin-bottom: 1rem;
  }
  label {
    display: block;
    margin-bottom: 0.3rem;
  }
  input {
    width: 100%;
    padding: 0.5rem;
    border: none;
    border-radius: 6px;
    background: #3b3b3b;
    color: #fff;
  }
  .auth-btn {
    width: 100%;
    padding: 0.6rem;
    background-color: #800000;
    color: white;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    transition: 0.3s;
  }
  .auth-btn:hover {
    background-color: #a00000;
  }
  .error {
    color: #ff7070;
    margin-top: 1rem;
    text-align: center;
  }
  </style>
  
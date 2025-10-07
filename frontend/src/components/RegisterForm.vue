<template>
    <form @submit.prevent="handleRegister">
      <div class="form-group">
        <label>Имя</label>
        <input v-model="name" type="text" required />
      </div>
  
      <div class="form-group">
        <label>E-mail</label>
        <input v-model="email" type="email" required />
      </div>
  
      <div class="form-group">
        <label>Пароль</label>
        <input v-model="password" type="password" required />
      </div>
  
      <div class="form-group">
        <label>Роль</label>
        <select v-model="role" required>
          <option disabled value="">Выберите роль</option>
          <option value="Manager">Менеджер</option>
          <option value="Engineer">Инженер</option>
          <option value="Observer">Наблюдатель</option>
        </select>
      </div>
  
      <button type="submit" class="auth-btn">Зарегистрироваться</button>
      <p v-if="error" class="error">{{ error }}</p>
    </form>
  </template>
  
  <script setup>
  import { ref } from "vue";
  import { useRouter } from "vue-router";
  import { useAuthStore } from "../store/auth";
  
  const name = ref("");
  const email = ref("");
  const password = ref("");
  const role = ref("");
  const error = ref("");
  const router = useRouter();
  const auth = useAuthStore();
  
  const handleRegister = async () => {
    try {
      await auth.register(name.value, email.value, password.value, role.value);
      router.push("/projects");
    } catch (err) {
      error.value =
        err.response?.data?.message || "Ошибка регистрации. Попробуйте снова.";
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
  input, select {
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
  
import { defineStore } from "pinia";
import { login, register } from "../api/authApi";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: null,
    token: localStorage.getItem("token") || null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    async login(email, password) {
      const data = await login(email, password);
      this.token = data.token;
      this.user = data.user;
      localStorage.setItem("token", data.token);
    },
    async register(name, email, password, role) {
      const data = await register(name, email, password, role);
      this.token = data.token;
      this.user = data.user;
      localStorage.setItem("token", data.token);
    },
    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem("token");
    },
  },
});

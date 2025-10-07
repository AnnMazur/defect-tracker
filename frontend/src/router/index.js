import { createRouter, createWebHistory } from "vue-router";
import Login from "../pages/LoginPage.vue";
import Register from "../pages/Register.vue";
import Projects from "../pages/ProjectsPage.vue";

const routes = [
  { path: "/login", component: Login },
  { path: "/register", component: Register },
  { path: "/projects", component: Projects },
  { path: "/", redirect: "/login" },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

import apiClient from "./client";

export const login = async (email, password) => {
  const response = await apiClient.post("/auth/login", { email, password });
  return response.data; // ожидаем { token, user }
};

export const register = async (name, email, password, role) => {
  const response = await apiClient.post("/auth/register", {
    name,
    email,
    password,
    role,
  });
  return response.data;
};

export const getCurrentUser = async () => {
  const response = await apiClient.get("/users/me");
  return response.data;
};

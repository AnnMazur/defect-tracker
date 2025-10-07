import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:5216/api", 
  headers: {
    "Content-Type": "application/json",
  },
});

// Добавляем JWT токен в заголовки при каждом запросе
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;

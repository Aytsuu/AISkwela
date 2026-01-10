import axios from 'axios';
import Cookies from 'js-cookie';

export const api = axios.create({
  baseURL: process.env.NODE_ENV == 'development' ? 'http://localhost:5251' : import.meta.env.NEXT_API_URL
})

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error?.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Call refresh token
        const oldAccessToken = Cookies.get('accessToken');
        const oldRefreshToken = Cookies.get('refreshToken');

        const response = await api.post('api/auth/refresh-token', {
          accessToken: oldAccessToken,
          refreshToken: oldRefreshToken
        });

        const { accessToken, refreshToken } = response.data

        const sixtyMinutes = new Date(new Date().getTime() + 10 * 60 * 1000);
        Cookies.set('accessToken', accessToken, { expires: sixtyMinutes });
        Cookies.set('refreshToken', refreshToken);

        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return api(originalRequest);
        
      } catch (refreshError) {
        Cookies.remove('accessToken');
        Cookies.remove('refreshToken');
        window.location.href = '/authentication/login';
      }
    }
  }
)
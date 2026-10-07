import { defineStore } from 'pinia'
import { authService } from '@/services/api'
import { ref } from 'vue'
import router from '@/router'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(localStorage.getItem('token'))
  const error = ref(null)

  async function login(email, password) {
    try {
      error.value = null
      const response = await authService.login(email, password)
      token.value = response.token
      // After successful login, redirect to home
      router.push('/')
    } catch (err) {
      error.value = err.response?.data?.message || 'An error occurred during login'
      throw error.value
    }
  }

  async function register(name, email, password) {
    try {
      error.value = null
      const response = await authService.register(name, email, password)
      token.value = response.token
      // After successful registration, redirect to home
      router.push('/')
    } catch (err) {
      error.value = err.response?.data?.message || 'An error occurred during registration'
      throw error.value
    }
  }

  function logout() {
    user.value = null
    token.value = null
    authService.logout()
    router.push('/login')
  }

  return {
    user,
    token,
    error,
    login,
    register,
    logout,
  }
})
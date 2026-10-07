<template>
  <v-app>
    <!-- Top Bar -->
    <v-app-bar color="primary">
      <!-- Left side -->
      <router-link to="/" class="text-decoration-none">
        <span class="text-white text-h6">Social Media App</span>
      </router-link>

      <v-spacer></v-spacer>

      <!-- Right side -->
      <template v-if="authStore.token">
        <v-btn
          v-if="isAdminUser"
          to="/admin"
          class="text-white mr-2"
          variant="text"
        >
          ADMIN
        </v-btn>
        <v-btn
          @click="logout"
          class="text-white"
          variant="text"
        >
          LOGOUT
        </v-btn>
      </template>
      <template v-else>
        <v-btn
          to="/login"
          class="text-white mr-2"
          variant="text"
        >
          LOGIN
        </v-btn>
        <v-btn
          to="/register"
          class="text-white"
          variant="text"
        >
          REGISTER
        </v-btn>
      </template>
    </v-app-bar>

    <!-- Main Content -->
    <v-main>
      <router-view></router-view>
    </v-main>

    <!-- Footer -->
    <v-footer app color="primary" class="d-flex justify-center pa-4">
      <span class="text-white">&copy; {{ new Date().getFullYear() }} Social Media App</span>
    </v-footer>
  </v-app>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()
const isAdminUser = ref(false)

onMounted(() => {
  checkAdminStatus()
})

function checkAdminStatus() {
  const token = localStorage.getItem('token')
  if (token) {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]))
      console.log('Token payload:', payload)
      isAdminUser.value = payload.role === 'ADMIN'
    } catch (error) {
      console.error('Error checking admin status:', error)
    }
  }
}

function logout() {
  localStorage.removeItem('token')
  authStore.logout()
  router.push('/login')
}
</script>

<style scoped>
.text-white {
  color: white !important;
}

.text-decoration-none {
  text-decoration: none;
}

.v-btn {
  letter-spacing: normal !important;
  text-transform: uppercase !important;
}
</style>
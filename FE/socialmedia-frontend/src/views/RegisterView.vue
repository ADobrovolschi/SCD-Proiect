<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Register</v-toolbar-title>
          </v-toolbar>
          <v-card-text>
            <v-form @submit.prevent="handleSubmit" v-model="isFormValid">
              <v-text-field
                v-model="name"
                label="Name"
                prepend-icon="mdi-account"
                :rules="nameRules"
                required
              />
              <v-text-field
                v-model="email"
                label="Email"
                prepend-icon="mdi-email"
                type="email"
                :rules="emailRules"
                required
              />
              <v-text-field
                v-model="password"
                label="Password"
                prepend-icon="mdi-lock"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showPassword ? 'text' : 'password'"
                @click:append="showPassword = !showPassword"
                :rules="passwordRules"
                required
              />
              <v-alert
                v-if="error"
                type="error"
                class="mb-4"
              >
                {{ error }}
              </v-alert>
              <v-btn
                type="submit"
                color="primary"
                block
                class="mt-4"
                :loading="loading"
                :disabled="!isFormValid || loading"
              >
                Register
              </v-btn>
            </v-form>
          </v-card-text>
          <v-card-text class="text-center">
            Already have an account?
            <v-btn
              variant="text"
              color="primary"
              @click="$router.push('/login')"
            >
              Login here
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const name = ref('')
const email = ref('')
const password = ref('')
const showPassword = ref(false)
const loading = ref(false)
const error = ref('')
const isFormValid = ref(false)

const nameRules = [
  v => !!v || 'Name is required',
  v => v.length >= 2 || 'Name must be at least 2 characters'
]

const emailRules = [
  v => !!v || 'Email is required',
  v => /.+@.+\..+/.test(v) || 'Email must be valid'
]

const passwordRules = [
  v => !!v || 'Password is required',
  v => v.length >= 6 || 'Password must be at least 6 characters'
]

async function handleSubmit() {
  if (!isFormValid.value) return

  loading.value = true
  error.value = ''

  try {
    await authStore.register(name.value, email.value, password.value)
  } catch (err) {
    error.value = err
  } finally {
    loading.value = false
  }
}
</script>
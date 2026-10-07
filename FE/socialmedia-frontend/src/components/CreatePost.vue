<template>
  <v-card class="mb-6">
    <v-card-title>Create New Post</v-card-title>
    <v-card-text>
      <v-form @submit.prevent="handleSubmit" v-model="isFormValid">
        <v-text-field
          v-model="title"
          label="Title"
          :rules="titleRules"
          required
        />
        <v-textarea
          v-model="content"
          label="Content"
          :rules="contentRules"
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
          :loading="loading"
          :disabled="!isFormValid || loading"
        >
          Create Post
        </v-btn>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script setup>
import { ref } from 'vue'
import { usePostStore } from '@/stores/posts'

const postStore = usePostStore()
const title = ref('')
const content = ref('')
const loading = ref(false)
const error = ref('')
const isFormValid = ref(false)

const titleRules = [
  v => !!v || 'Title is required',
  v => v.length >= 3 || 'Title must be at least 3 characters'
]

const contentRules = [
  v => !!v || 'Content is required',
  v => v.length >= 10 || 'Content must be at least 10 characters'
]

async function handleSubmit() {
  if (!isFormValid.value) return

  loading.value = true
  error.value = ''

  try {
    await postStore.createPost({
      title: title.value,
      content: content.value,
      status: 'PENDING'
    })
    // Clear form after successful submission
    title.value = ''
    content.value = ''
    // Emit success event
    emit('post-created')
  } catch (err) {
    error.value = err
  } finally {
    loading.value = false
  }
}

const emit = defineEmits(['post-created'])
</script>
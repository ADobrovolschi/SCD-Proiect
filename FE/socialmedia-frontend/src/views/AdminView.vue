<template>
  <v-container>
    <h2>Admin Panel</h2>

    <!-- Pending Posts Section -->
    <div class="mt-4">
      <h3>Pending Posts</h3>
      
      <div v-if="loading" class="text-center my-4">
        <v-progress-circular indeterminate></v-progress-circular>
      </div>

      <div v-else>
        <v-card
          v-for="post in pendingPosts"
          :key="post.id"
          class="mt-4"
          elevation="2"
        >
          <v-card-title>{{ post.title }}</v-card-title>
          <v-card-subtitle>
            By {{ post.user?.name || 'Anonymous' }}
            on {{ new Date(post.createdOn).toLocaleString() }}
          </v-card-subtitle>
          
          <v-card-text>{{ post.content }}</v-card-text>
          
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              color="error"
              variant="outlined"
              @click="rejectPost(post.id)"
            >
              Reject
            </v-btn>
            <v-btn
              color="success"
              class="ml-2"
              @click="approvePost(post.id)"
            >
              Approve
            </v-btn>
          </v-card-actions>
        </v-card>

        <div v-if="pendingPosts.length === 0" class="text-center my-4">
          No pending posts to review
        </div>
      </div>
    </div>

    <!-- Users Section -->
    <div class="mt-6">
      <h3>Users Management</h3>
      
      <v-data-table
        :headers="headers"
        :items="users"
        :loading="loadingUsers"
        class="mt-4"
      >
        <template v-slot:item.actions="{ item }">
          <v-btn
            color="error"
            variant="text"
            @click="confirmBanUser(item)"
            icon
          >
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </div>

    <!-- Ban User Dialog -->
    <v-dialog v-model="showBanDialog" max-width="300px">
      <v-card>
        <v-card-title>Ban User?</v-card-title>
        <v-card-text>
          Are you sure you want to ban this user? All their content will be removed.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" text @click="showBanDialog = false">Cancel</v-btn>
          <v-btn color="error" @click="banUser">Ban User</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

// State
const loading = ref(false)
const loadingUsers = ref(false)
const pendingPosts = ref([])
const users = ref([])
const showBanDialog = ref(false)
const selectedUser = ref(null)

// Table headers
const headers = [
  { title: 'Name', key: 'name' },
  { title: 'Email', key: 'email' },
  { title: 'Role', key: 'role' },
  { title: 'Actions', key: 'actions', sortable: false }
]

// Methods
async function fetchPendingPosts() {
  loading.value = true
  try {
    const response = await fetch('http://localhost:8083/post/pending', {
      headers: {
        'Authorization': `Bearer ${authStore.token}`
      }
    })
    const data = await response.json()
    pendingPosts.value = data
  } catch (error) {
    console.error('Error fetching pending posts:', error)
  } finally {
    loading.value = false
  }
}

async function fetchUsers() {
  loadingUsers.value = true
  try {
    const response = await fetch('http://localhost:8083/admin/users', {
      headers: {
        'Authorization': `Bearer ${authStore.token}`
      }
    })
    const data = await response.json()
    users.value = data
  } catch (error) {
    console.error('Error fetching users:', error)
  } finally {
    loadingUsers.value = false
  }
}

async function approvePost(postId) {
  try {
    await fetch(`http://localhost:8083/admin/posts/${postId}/approve`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${authStore.token}`
      }
    })
    await fetchPendingPosts()
  } catch (error) {
    console.error('Error approving post:', error)
  }
}

async function rejectPost(postId) {
  try {
    await fetch(`http://localhost:8083/admin/posts/${postId}/reject`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${authStore.token}`
      }
    })
    await fetchPendingPosts()
  } catch (error) {
    console.error('Error rejecting post:', error)
  }
}

function confirmBanUser(user) {
  selectedUser.value = user
  showBanDialog.value = true
}

async function banUser() {
  if (!selectedUser.value) return
  
  try {
    await fetch(`http://localhost:8083/admin/users/${selectedUser.value.id}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${authStore.token}`
      }
    })
    await fetchUsers()
    showBanDialog.value = false
  } catch (error) {
    console.error('Error banning user:', error)
  } finally {
    selectedUser.value = null
  }
}

// Lifecycle
onMounted(() => {
  fetchPendingPosts()
  fetchUsers()
})
</script>
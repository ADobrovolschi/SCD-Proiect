<template>
  <v-container>
    <div v-if="authStore.token">
      <h2>Create a New Post</h2>
      <div class="mb-4 pa-4 bg-white">
        <v-form @submit.prevent="createPost">
          <v-text-field
            v-model="newPost.title"
            label="Post Title"
            class="mb-4"
            hide-details
          ></v-text-field>
          
          <v-textarea
            v-model="newPost.content"
            label="Post Content"
            class="mb-4"
            hide-details
          ></v-textarea>
          
          <v-btn 
            type="submit" 
            color="primary"
            :loading="loading"
          >
            Create Post
          </v-btn>
        </v-form>
      </div>

      <h2>Recent Posts</h2>
      
      <div v-for="post in postStore.posts" :key="post.id" class="mb-4 pa-4 bg-white">
        <div class="d-flex justify-space-between align-center">
          <h3>{{ post.title }}</h3>
          <v-chip
            :color="getStatusColor(post.status)"
            size="small"
          >
            {{ post.status }}
          </v-chip>
        </div>
        
        <p>{{ post.content }}</p>
        <div class="text-caption text-grey">
          Posted on {{ new Date(post.createdOn).toLocaleString() }}
        </div>

        <!-- Comments Section -->
        <div class="mt-4">
          <div class="d-flex justify-space-between align-center">
            <div>Comments ({{ post.comments?.length || 0 }})</div>
          </div>
          
          <!-- Add Comment Form -->
          <div class="mt-2">
            <v-text-field
              v-model="newComments[post.id]"
              label="Add a comment"
              append-inner-icon="mdi-send"
              @click:append-inner="addComment(post.id)"
              @keyup.enter="addComment(post.id)"
              hide-details
              density="compact"
            ></v-text-field>
          </div>

          <!-- Comments List -->
          <div v-if="post.comments?.length" class="mt-3">
            <div v-for="comment in post.comments" :key="comment.id" class="pa-2 mb-2 bg-grey-lighten-4 rounded">
              <div class="d-flex justify-space-between align-center">
                <div class="d-flex align-center">
                  <v-avatar color="primary" size="24" class="mr-2">
                    {{ (comment.user?.name || 'A').charAt(0) }}
                  </v-avatar>
                  <span class="font-weight-medium">{{ comment.user?.name || 'Anonymous' }}</span>
                </div>
                
                <!-- Edit/Delete Buttons for Comment Owner -->
                <div v-if="comment.user?.id === currentUserId">
                  <v-btn
                    icon="mdi-pencil"
                    size="small"
                    variant="text"
                    @click="startEditComment(comment)"
                  ></v-btn>
                  <v-btn
                    icon="mdi-delete"
                    size="small"
                    variant="text"
                    color="error"
                    @click="deleteComment(comment.id)"
                  ></v-btn>
                </div>
              </div>
              
              <!-- Edit Mode -->
              <v-text-field
                v-if="editingComment?.id === comment.id"
                v-model="editingComment.content"
                class="mt-2"
                density="compact"
                hide-details
                @keyup.enter="updateComment(comment.id)"
                append-inner-icon="mdi-check"
                @click:append-inner="updateComment(comment.id)"
              ></v-text-field>
              
              <!-- Display Mode -->
              <div v-else class="mt-1">
                {{ comment.content }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { usePostStore } from '@/stores/posts'

const authStore = useAuthStore()
const postStore = usePostStore()
const loading = ref(false)
const newPost = ref({ title: '', content: '' })
const newComments = ref({})
const currentUserId = ref(null)
const editingComment = ref(null)

function getStatusColor(status) {
  switch (status) {
    case 'PUBLISHED':
      return 'success'
    case 'PENDING':
      return 'warning'
    case 'REMOVED':
      return 'error'
    default:
      return 'grey'
  }
}

function startEditComment(comment) {
  editingComment.value = {
    id: comment.id,
    content: comment.content
  }
}

async function updateComment(commentId) {
  if (!editingComment.value || !editingComment.value.content) return
  
  try {
    await postStore.updateComment(commentId, {
      content: editingComment.value.content
    })
    editingComment.value = null
    await postStore.fetchPosts()
  } catch (error) {
    console.error('Failed to update comment:', error)
  }
}

async function deleteComment(commentId) {
  if (!confirm('Are you sure you want to delete this comment?')) return
  
  try {
    await postStore.deleteComment(commentId)
    await postStore.fetchPosts()
  } catch (error) {
    console.error('Failed to delete comment:', error)
  }
}

onMounted(async () => {
  if (authStore.token) {
    await postStore.fetchPosts()
    // Get user ID from token
    const token = localStorage.getItem('token')
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]))
        currentUserId.value = payload.id
      } catch (error) {
        console.error('Error decoding token:', error)
      }
    }
  }
})

// Existing functions...
const createPost = async () => {
  if (!newPost.value.title || !newPost.value.content) return
  
  loading.value = true
  try {
    await postStore.createPost(newPost.value)
    newPost.value = { title: '', content: '' }
    await postStore.fetchPosts()
  } catch (error) {
    console.error('Failed to create post:', error)
  } finally {
    loading.value = false
  }
}

const addComment = async (postId) => {
  if (!newComments.value[postId]) return
  
  try {
    await postStore.addComment(postId, {
      content: newComments.value[postId]
    })
    newComments.value[postId] = ''
    await postStore.fetchPosts()
  } catch (error) {
    console.error('Failed to add comment:', error)
  }
}
</script>

<style scoped>
.bg-white {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
}
</style>
<template>
  <v-card class="mb-4">
    <v-card-title class="d-flex justify-space-between align-center">
      {{ post.title }}
      <v-menu v-if="isUserPost">
        <template v-slot:activator="{ props }">
          <v-btn
            icon
            v-bind="props"
          >
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item @click="handleDelete">
            <v-list-item-title>Delete</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-card-title>

    <v-card-subtitle>
      Posted by {{ post.user?.name || 'Anonymous' }}
      on {{ new Date(post.createdOn).toLocaleDateString() }}
    </v-card-subtitle>

    <v-card-text>{{ post.content }}</v-card-text>

    <!-- Comments Section -->
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-title>
          Comments ({{ post.comments?.length || 0 }})
        </v-expansion-panel-title>
        <v-expansion-panel-text>
          <!-- Add Comment Form -->
          <v-form @submit.prevent="addComment" class="mb-4">
            <v-textarea
              v-model="newComment"
              label="Add a comment"
              rows="2"
              hide-details
              class="mb-2"
            />
            <v-btn
              type="submit"
              color="primary"
              :disabled="!newComment.trim()"
              :loading="commentLoading"
              size="small"
            >
              Post Comment
            </v-btn>
          </v-form>

          <!-- Comments List -->
          <v-list>
            <v-list-item
              v-for="comment in post.comments"
              :key="comment.id"
            >
              <template v-slot:prepend>
                <v-avatar color="primary" size="36">
                  <span class="text-h6 white--text">
                    {{ comment.user?.name?.charAt(0) || 'A' }}
                  </span>
                </v-avatar>
              </template>

              <v-list-item-title>
                {{ comment.user?.name || 'Anonymous' }}
              </v-list-item-title>
              
              <v-list-item-subtitle>
                {{ new Date(comment.createdOn).toLocaleDateString() }}
              </v-list-item-subtitle>
              
              <v-list-item-text>
                {{ comment.content }}
              </v-list-item-text>
            </v-list-item>
          </v-list>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>

  <!-- Delete Confirmation Dialog -->
  <v-dialog v-model="showDeleteDialog" width="auto">
    <v-card>
      <v-card-title>Delete Post?</v-card-title>
      <v-card-text>
        Are you sure you want to delete this post? This action cannot be undone.
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="grey" @click="showDeleteDialog = false">Cancel</v-btn>
        <v-btn 
          color="error" 
          @click="confirmDelete"
          :loading="deleteLoading"
        >
          Delete
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup>
import { ref, computed } from 'vue'
import { usePostStore } from '@/stores/posts'
import { useAuthStore } from '@/stores/auth'

const props = defineProps({
  post: {
    type: Object,
    required: true
  }
})

const authStore = useAuthStore()
const postStore = usePostStore()
const newComment = ref('')
const commentLoading = ref(false)
const showDeleteDialog = ref(false)
const deleteLoading = ref(false)

// Check if the current user is the author of the post
const isUserPost = computed(() => {
  return props.post.user?.id === authStore.user?.id
})

async function addComment() {
  if (!newComment.value.trim()) return

  commentLoading.value = true
  try {
    await postStore.addComment(props.post.id, {
      content: newComment.value
    })
    newComment.value = ''
  } catch (error) {
    console.error('Failed to add comment:', error)
  } finally {
    commentLoading.value = false
  }
}

function handleDelete() {
  showDeleteDialog.value = true
}

async function confirmDelete() {
  deleteLoading.value = true
  try {
    await postStore.deletePost(props.post.id)
    showDeleteDialog.value = false
  } catch (error) {
    console.error('Failed to delete post:', error)
  } finally {
    deleteLoading.value = false
  }
}
</script>
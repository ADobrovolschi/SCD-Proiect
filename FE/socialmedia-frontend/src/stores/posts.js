import { defineStore } from 'pinia'
import { ref } from 'vue'
import { postService, commentService } from '@/services/api'

export const usePostStore = defineStore('posts', () => {
  const posts = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function fetchPosts() {
    try {
      loading.value = true
      const response = await postService.getAllPosts()
      posts.value = response.data || []
    } catch (err) {
      console.error('Error fetching posts:', err)
      error.value = err.response?.data?.message || 'Failed to fetch posts'
    } finally {
      loading.value = false
    }
  }

  async function createPost(postData) {
    try {
      loading.value = true
      await postService.createPost({
        ...postData,
        status: 'PENDING'
      })
      await fetchPosts()
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to create post'
      throw error.value
    } finally {
      loading.value = false
    }
  }

  async function addComment(postId, commentData) {
    try {
      await commentService.createComment({
        content: commentData.content,
        post: { id: postId }
      })
      await fetchPosts()
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to add comment'
      throw error.value
    }
  }

  async function updateComment(commentId, commentData) {
    try {
      await commentService.updateComment(commentId, commentData)
      await fetchPosts()
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to update comment'
      throw error.value
    }
  }

  async function deleteComment(commentId) {
    try {
      await commentService.deleteComment(commentId)
      await fetchPosts()
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to delete comment'
      throw error.value
    }
  }

  return {
    posts,
    loading,
    error,
    fetchPosts,
    createPost,
    addComment,
    updateComment,
    deleteComment
  }
})
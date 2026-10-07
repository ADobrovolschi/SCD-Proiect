import axios from 'axios'

const API_URL = 'http://localhost:8083'

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Add a request interceptor to add the JWT token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

export const authService = {
  async login(email, password) {
    const response = await api.post('/auth/login', { email, password })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token)
    }
    return response.data
  },

  async register(name, email, password) {
    const response = await api.post('/auth/register', { name, email, password })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token)
    }
    return response.data
  },

  logout() {
    localStorage.removeItem('token')
  }
}

export const postService = {
  async getAllPosts() {
    return await api.get('/post')
  },

  async getPublishedPosts() {
    return await api.get('/post/published')
  },

  async createPost(post) {
    return await api.post('/post', post)
  },

  async updatePost(id, post) {
    return await api.put(`/post/${id}`, post)
  },

  async deletePost(id) {
    return await api.delete(`/post/${id}`)
  },

  async searchPosts(keyword) {
    return await api.get(`/post/search?keyword=${keyword}`)
  }
}

export const commentService = {
  async getPostComments(postId) {
    return await api.get(`/comment/post/${postId}`)
  },

  async createComment(comment) {
    return await api.post('/comment', comment)
  },

  async updateComment(id, comment) {
    return await api.put(`/comment/${id}`, comment)
  },

  async deleteComment(id) {
    return await api.delete(`/comment/${id}`)
  }
}

export default api
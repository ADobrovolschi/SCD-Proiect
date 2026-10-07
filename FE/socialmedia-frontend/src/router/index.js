import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AdminView from '../views/AdminView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue')
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminView,
      beforeEnter: (to, from, next) => {
        const token = localStorage.getItem('token')
        if (!token) {
          next('/login')
          return
        }
        
        try {
          const payload = JSON.parse(atob(token.split('.')[1]))
          if (payload.role === 'ADMIN') {
            next()
          } else {
            next('/')
          }
        } catch (error) {
          console.error('Error checking admin status:', error)
          next('/login')
        }
      }
    }
  ]
})

export default router
import { createRouter, createWebHistory } from 'vue-router';
import NoteListView from '@/views/notes/NoteListView.vue';
import NoteEditView from '@/views/notes/NoteEditView.vue';
import LoginView from '@/views/account/UserLoginView.vue';
import RegisterView from '@/views/account/UserRegisterView.vue';
import home from '@/views/home.vue';
import setting from '@/views/account/setting.vue';
import userProfile from '@/views/account/Profile.vue';
import DocumentSignature from '@/views/signature.vue';
import VideoPlay from '@/views/video/VideoPlay.vue';
import VideoUpload from '@/views/video/VideoUpload.vue';

const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginView
  }
  ,
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  }
  ,
  {
    path: '/notes',
    name: 'notes',
    component: NoteListView
  },
  {
    path: '/notes/new',
    name: 'new-note',
    component: NoteEditView
  },
  {
    path: '/notes/:id/edit',
    name: 'edit-note',
    component: NoteEditView
  }
  ,
  {
    path: '/home',
    name: 'home',
    component: home,
    children: [
      {
        path: 'setting',
        name: 'setting',
        component: setting

      },
      {
        path: 'profile',
        name: 'profile',
        component: userProfile

      }
    ]
  }
  ,
  {
    path: '/signature',
    name: 'DocumentSignature',
    component: DocumentSignature
  }
  ,
  {
    path: '/VideoPlay',
    name: VideoPlay,
    component: VideoPlay
  },
  {
    path:'/VideoUpload',
    name: VideoUpload,
    component:VideoUpload
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
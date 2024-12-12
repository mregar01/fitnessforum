import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainPage from './MainPage';
import PostDetail from './PostDetail';
import UserDetail from './UserDetail';
import TagPage from './TagPage';
import NewPost from './NewPost';
import SearchResult from './SearchResult';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/posts/:id" element={<PostDetail />} />
        <Route path="/users/:userid" element={<UserDetail />} />
        <Route path="/posts/tag/:tagName" element={<TagPage />} />
        <Route path="/posts/new" element={<NewPost />} />
        <Route path="/search/:searchString/:useOpenSearch" element={<SearchResult />} />
      </Routes>
    </Router>
  );
};

export default App;


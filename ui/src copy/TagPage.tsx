import React, { useState, useEffect, useCallback } from 'react';
import { Link, useParams, useLocation } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import PostList from './PostList'

interface Post {
  id: number;
  votes: number;
  answerCount: number;
  viewCount: number;
  title: string;
  tags: string;
  parentId: number;
  parentTitle: string;
  parentTags: string;
  acceptedAnswerId: number;
}


const TagPage: React.FC = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const location = useLocation();
  const { tagName } = useParams<{ tagName: string }>();
  const pageSize = 25;
  
  const fetchPosts = useCallback(async () => {
    try {
      const response = await fetch(`/api/posts/tag/${tagName}?page=${currentPage}`);
      const responseData: Post[] = await response.json();
      setPosts(responseData);
    } catch (error) {
      console.error(error);
    }
  }, [currentPage, tagName]);
  

  useEffect(() => {
    fetchPosts();
  }, [fetchPosts]);

  useEffect(() => {
    setCurrentPage(1);
  }, [location]);

  const goToNextPage = () => {
    setCurrentPage((prevPage) => prevPage + 1);
    window.scrollTo(0, 0);
  };

  const goToPreviousPage = () => {
    setCurrentPage((prevPage) => prevPage - 1);
    window.scrollTo(0, 0);
  };

  const goToFirstPage = () => {
    setCurrentPage(1);
    window.scrollTo(0, 0);
  };

  return (
    <div>
      <Link to="/">
          <button className='btn btn-primary mx-4'>Go to Main Page</button>
      </Link>
      <div className='container justify-content-center align-items-center bg-secondary sticky-top my-3'>
        <h1 className='container text-center align-middle p-3'>Posts Tagged {tagName} Page {currentPage}</h1>
      </div>

      <PostList posts={posts} type="normal"/>;

      <div className="container justify-content-center m-5 sticky-bottom row">
        <button
          className="btn btn-primary col-3 m-2"
          disabled={currentPage === 1}
          onClick={goToFirstPage}
        >
          First
        </button>
        <button
          className="btn btn-primary col-3 m-2"
          disabled={currentPage === 1}
          onClick={goToPreviousPage}
        >
          Previous
        </button>
        <button 
          className="btn btn-primary col-3 m-2" 
          disabled={posts.length < pageSize}
          onClick={goToNextPage}
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default TagPage;

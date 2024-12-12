import React, { useState, useEffect, useCallback } from 'react';
import { Link, useParams, useLocation } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import PostList from './PostList'
import LoadingOverlay from './LoadingOverlay'
import SearchList from './SearchList'

// interface Post {
//   id: number;
//   votes: number;
//   answerCount: number;
//   viewCount: number;
//   title: string;
//   tags: string;
//   parentId: number;
//   parentTitle: string;
//   parentTags: string;
//   acceptedAnswerId: number;
// }

interface SearchResultItem {
  questionId: number
  id: number;
  body: string;
  title: string;
  tags: string;
  voteCount: number;
  viewCount: number;
  postTypeId: number
  acceptedAnswerId: number;
  answerCount: number;
}


const SearchResult: React.FC = () => {
  const [posts, setPosts] = useState<SearchResultItem[]>([]);
  const { searchString } = useParams<{ searchString: string }>();
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [isLoading, setIsLoading] = useState(false);
  const location = useLocation();
  const pageSize = 15;
  var { useOpenSearch } = useParams<{ useOpenSearch: string }>();
  const [isUseOpenSearch, setIsUseOpenSearch] = useState<boolean>(useOpenSearch === 'true')



  const handleSearch = () => {
    if (searchQuery.trim() !== '') {
      const encodedQuery = encodeURIComponent(searchQuery.trim());
      window.location.href = `/search/${encodedQuery}/${isUseOpenSearch}`;
    }
  };

  
  const fetchPosts = useCallback(async () => {
    try {
      setIsLoading(true);
      const searchType = isUseOpenSearch ? 'opensearch' : 'sql';
      const response = await fetch(`/api/search?query=${searchString}&page=${currentPage}&name=${searchType}`);
      const responseData: SearchResultItem[] = await response.json();
      setPosts(responseData);
      setIsLoading(false);
    } catch (error) {
      console.error(error);
    }
  }, [searchString, currentPage]);
  

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

  if (isLoading) {
    return <LoadingOverlay message="Loading Results..." />;
  }


  return (
    <div>
      <Link to="/">
          <button className='btn btn-primary mx-4'>Go to Main Page</button>
      </Link>
      <div className='container justify-content-center align-items-center bg-secondary sticky-top my-3'>
        <h1 className='container text-center align-middle p-3'>Results for: {searchString}</h1>
      </div>
      <div className='container d-flex align-items-center justify-content-center my-2'>
        <input
          type='text'
          className='form-control'
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          placeholder='Search...'
        />
        <button className='btn btn-primary mx-2' onClick={handleSearch}>
          Search
        </button>
        <button
          className={`btn ${isUseOpenSearch ? 'btn-success' : 'btn-secondary'} mx-2`}
          onClick={() => setIsUseOpenSearch(!isUseOpenSearch)}
        >
          {isUseOpenSearch ? 'Open Search On' : 'Open Search Off'}
        </button>
        
      </div>

      {/* <PostList posts={posts} type="search"/> */}
      <SearchList posts={posts} />

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

export default SearchResult;

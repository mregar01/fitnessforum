import React from 'react';
import TagList from './TagList';

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

interface PostListProps {
  posts: SearchResultItem[];
}

const SearchList: React.FC<PostListProps> = ({ posts }) => {
  if (posts.length === 0) {
    return <h1 className='text-center container'>No posts found</h1>
  }


  return (
    <div>
      {posts.map((data) => {
        const postLink = `/posts/${data.questionId}`;
        const postTitle =
          data.postTypeId === 1 ? (
              <span>
                <svg
                  aria-hidden="true"
                  className="svg-icon iconQuestion"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                >
                  <path
                    d="m4 15-3 3V4c0-1.1.9-2 2-2h12c1.09 0 2 .91 2 2v9c0 1.09-.91 2-2 2H4Zm7.75-3.97c.72-.83.98-1.86.98-2.94 0-1.65-.7-3.22-2.3-3.83a4.41 4.41 0 0 0-3.02 0 3.8 3.8 0 0 0-2.32 3.83c0 1.29.35 2.29 1.03 3a3.8 3.8 0 0 0 2.85 1.07c.62 0 1.2-.11 1.71-.34.65.44 1 .68 1.06.7.23.13.46.23.7.3l.59-1.13a5.2 5.2 0 0 1-1.28-.66Zm-1.27-.9a5.4 5.4 0 0 0-1.5-.8l-.45.9c.33.12.66.29.98.5-.2.07-.42.11-.65.11-.61 0-1.12-.23-1.52-.68-.86-1-.86-3.12 0-4.11.8-.9 2.35-.9 3.15 0 .9 1.01.86 3.03-.01 4.08Z"
                  />
                </svg>
                {data.title ? data.title :("[Post Deleted]")}
              </span>
            ) : (
            <span>
              <svg 
                aria-hidden="true" 
                className="svg-icon iconAnswer" 
                width="24"
                height="24"
                viewBox="0 0 24 24"
              >
                <path 
                  d="M14 15H3c-1.09 0-2-.91-2-2V4c0-1.1.9-2 2-2h12c1.09 0 2 .91 2 2v14l-3-3Zm-1.02-3L9.82 4H8.14l-3.06 8h1.68l.65-1.79h3.15l.69 1.79h1.73Zm-2.93-3.12H7.9l1.06-2.92 1.09 2.92Z">
                </path>
              </svg>
              {data.title ? data.title :("[Parent Post Deleted]")}
            </span>
            );

        const postTags = data.tags;
        const hasAcceptedAnswer = data.acceptedAnswerId !== null;
        const hasAnswers = (data.answerCount !== null && data.answerCount !== 0) || data.postTypeId === 1;

        return (
          <div key={data.id}>
            <div className="container">
              <div className="row align-items-start">
                <div className="col-lg-2 col-4 offset-1 text-center">
                  {data.voteCount ? data.voteCount : ("0")} votes <br />                                    
                  {data.postTypeId === 2 && data.acceptedAnswerId !== null ? (
                    <span className="background bg-success text-light rounded border-success p-1">
                      <svg
                        className="check-icon"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 14 14"
                        width="14"
                        height="14"
                      >
                        <path
                          fill="white"
                          d="M13 3.41 11.59 2 5 8.59 2.41 6 1 7.41l4 4 8-8Z"
                        />
                      </svg>
                      Accepted
                    </span>
                  ) : (
                    hasAnswers && (
                    <span className={`border p-1 ${hasAcceptedAnswer ? 'background bg-success text-light rounded border-success' : 'border-success text-success rounded'}`}>
                      {hasAcceptedAnswer && (
                        <svg
                          className="check-icon"
                          xmlns="http://www.w3.org/2000/svg"
                          viewBox="0 0 14 14"
                          width="14"
                          height="14"
                        >
                          <path
                            fill="white"
                            d="M13 3.41 11.59 2 5 8.59 2.41 6 1 7.41l4 4 8-8Z"
                          />
                        </svg>
                      )}
                      {data.answerCount} answers
                    </span>)
                  )}
                  <br />
                  {data.postTypeId === 1 && (
                    <span className="text-secondary">{data.viewCount} views</span>
                  )}
                  <br />
                </div>
                <div className="col-lg-8 col-7">
                  <h4>
                    <a href={postLink} className="col-8 mx-1" rel="noreferrer">
                      {postTitle}
                    </a>
                  </h4>
                  <TagList className="container" string={postTags} />
                  {/* {data.body} */}
                  <br />
                </div>
                <hr />
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );
};

export default SearchList;
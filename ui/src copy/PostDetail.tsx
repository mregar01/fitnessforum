import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import TagList from './TagList'
import RenderHTML from './RenderHTML';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import UpvoteButton from './UpvoteButton'
import DownvoteButton from './DownvoteButton';
import EditPost from './EditPost';
import EditResponse from './EditResponse'
import LoadingOverlay from './LoadingOverlay'
import PostUserInfo from './PostUserInfo'
import PrintComments from './PrintComments';


interface Comment {
  id: number;
  score: number;
  text: string;
  userDisplayName: string;
  userId: number;
  creationDate: string;
}

interface Response {
  id: number;
  title: string;
  votes: number;
  body: string;
  creationDate: string;
  ownerUserId: number;
  ownerDisplayName: string;
  ownerRep: number;
  ownerGoldBadges: number;
  ownerSilverBadges: number;
  ownerBronzeBadges: number;
  comments: Comment[];
  parentId: number;
}

interface Post {
  postItem: {
    id: number;
    title: string;
    votes: number;
    body: string;
    creationDate: string;
    ownerUserId: number;
    ownerDisplayName: string;
    ownerRep: number;
    ownerGoldBadges: number;
    ownerSilverBadges: number;
    ownerBronzeBadges: number;
    tags: string;
    comments: Comment[];
  };
  responses: Response[];
}

const PostDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [post, setPost] = useState<Post | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isCommenting, setItsCommenting] = useState(false);
  const [isCommentVisiblePost, setIsCommentVisiblePost] = useState(false);
  const [isCommentVisibleResponse, setIsCommentVisibleResponse] = useState<{ [responseId: number]: boolean }>({});
  const [commentTextPost, setCommentTextPost] = useState('');
  const [commentTextResponse, setCommentTextResponse] = useState('');
  const [responseText, setResponseText] = useState('');

  const isResponseValid = () => {
    return responseText.trim() !== '';
  }

  const isPostCommentValid = () => {
    return commentTextPost.trim() !== '';
  }

  const isResponseCommentValid = () => {
    return commentTextResponse.trim() !== '';
  }

  const handleCommentClickResponse = (responseId: number) => {
    setIsCommentVisibleResponse({[responseId]: true});
  };


  const handleCommentClickPost = () => {
    setIsCommentVisiblePost(true);
  }

  const handleCommentSubmit = async (postId: number, commentBody: string) => {
    setItsCommenting(true);
    try {
      await fetch('/api/comments/add', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          postId: postId,
          commentBody: commentBody,
        }),
        credentials: 'include',
      });
  
      const responseRefresh = await fetch(`/api/posts/${id}`);
      const postData: Post = await responseRefresh.json();
  
      setPost(postData);
      setCommentTextPost(''); // Clear the post comment text after submission
      setCommentTextResponse('');
      setIsCommentVisiblePost(false);
      setIsCommentVisibleResponse(prevState => ({
        ...prevState,
        [postId]: false,
      }));
      setItsCommenting(false);
    } catch (error) {
      console.error(error);
    }
  };
  

  
  // Button click event to add a comment to a response
  const handleCommentSubmitResponse = async (responseId: number) => {
    const response = post?.responses.find(response => response.id === responseId);
    if (response) {
      await handleCommentSubmit(responseId, commentTextResponse);
    }
  };
  
  

  const handleUpvote = (postId: number | undefined) => {
    setPost(prevPost => {
      if (prevPost && prevPost.postItem && prevPost.postItem.id === postId) {
        return {
          ...prevPost,
          postItem: {
            ...prevPost.postItem,
            votes: prevPost.postItem.votes + 1, // Increment the vote count
          },
        };
      }
      return prevPost;
    });
  };

  const handleUpvoteResponse = (response: Response) => {
    setPost(prevPost => {
      if (prevPost && prevPost.responses) {
        const updatedResponses = prevPost.responses.map(item => {
          if (item.id === response.id) {
            return {
              ...item,
              votes: item.votes + 1, // Increment the vote count
            };
          }
          return item;
        });
  
        return {
          ...prevPost,
          responses: updatedResponses,
        };
      }
      return prevPost;
    });
  };
  
  
  const handleDownvote = (postId: number | undefined) => {
    setPost(prevPost => {
      if (prevPost && prevPost.postItem && prevPost.postItem.id === postId) {
        return {
          ...prevPost,
          postItem: {
            ...prevPost.postItem,
            votes: prevPost.postItem.votes - 1, // Decrement the vote count
          },
        };
      }
      return prevPost;
    });
  };
  

  const handleDownvoteResponse = (response: Response) => {
    setPost(prevPost => {
      if (prevPost && prevPost.responses) {
        const updatedResponses = prevPost.responses.map(item => {
          if (item.id === response.id) {
            return {
              ...item,
              votes: item.votes - 1, // Increment the vote count
            };
          }
          return item;
        });
  
        return {
          ...prevPost,
          responses: updatedResponses,
        };
      }
      return prevPost;
    });
  };

  const submitResponse = async (parentId: number | undefined) => {
    try {
      setIsLoading(true);
      await fetch(`/api/posts/${parentId}/answers/add`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          parentId: parentId,
          responseBody: responseText,
        }),
        credentials: 'include',
      });
  
      const responseRefresh = await fetch(`/api/posts/${id}`);
      const postData: Post = await responseRefresh.json();
  
      setPost(postData);
      setResponseText('');
      setIsLoading(false);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const response = await fetch(`/api/posts/${id}`);
        const postData: Post = await response.json();
  
        setPost(postData);
        setIsLoading(false);
      } catch (error) {
        console.error(error);
      }
    };
  
    fetchPost();
  }, [id]);
  

  if (isLoading) {
    return <LoadingOverlay message="Loading Post..." />;
  }

  if(isCommenting) {
    return <LoadingOverlay message="Commenting..." />;
  }

  return (
    <div>
      <Link to="/">
          <button className='btn btn-primary mx-4'>Go to Main Page</button>
      </Link>
      <div className='container bg-light m-5'>
        <div className='row'>
          <div className='col-1 d-flex align-items-start justify-content-center'>
            <div className='d-flex flex-column align-items-center'>
              <UpvoteButton postId={post?.postItem.id} onUpvote={() => handleUpvote(post?.postItem.id)} />
              <b>{post?.postItem.votes}</b>
              <DownvoteButton postId={post?.postItem.id} onDownvote={() => handleDownvote(post?.postItem.id)} />
            </div>
          </div>

          <div className='col'>
            <h1>{post?.postItem.title}</h1>
              <br />
              <RenderHTML HTML={post?.postItem.body} />
              <TagList string={post?.postItem.tags} />

              <div className='row'>
                <div className='col-1 offset-lg-7 offset-md-5 offset-1'>
                  <EditPost POST={post?.postItem} />
                </div>
                <div className='col-7 offset-2 col-lg-3 col-md-5 offset-md-1 bg-secondary text-light p-2 rounded'>
                  <PostUserInfo item={post}/>
                </div>
              </div>
              <br />
              <PrintComments comments={post?.postItem.comments}/>
              {isCommentVisiblePost ? (
                <div>
                  <div className="row">
                    <div className="col-10 d-flex flex-column">
                      <textarea
                        rows={4}
                        value={commentTextPost}
                        onChange={(e) => setCommentTextPost(e.target.value)}
                      />
                      <small className="text-muted">Enter at least 15 characters</small>
                    </div>
                    <div className="col d-flex align-items-end my-2">
                      <button
                        className="mx-2 btn btn-secondary"
                        onClick={() => post?.postItem.id && handleCommentSubmit(post.postItem.id, commentTextPost)}
                        disabled={!isPostCommentValid()}
                      >
                        Add Comment
                      </button>
                    </div>
                  </div>
                </div>
              ) : (
                <div className='container my-2'>
                  <button onClick={() => handleCommentClickPost()} className="btn btn-secondary">Add a comment</button>
                </div>
              )}
          </div>
        </div>
      </div>

      <h2 className='container text-center'>{post?.responses.length} Answers</h2>
      {post?.responses && post?.responses.length > 0 ? (
        post?.responses.map((response) => (
          <div className='container bg-light m-5' key={response.id}>
            <div className='row'>
              <div className='col-1 d-flex align-items-start justify-content-center'>
                <div className='d-flex flex-column align-items-center'>
                  <UpvoteButton postId={response.id} onUpvote={() => handleUpvoteResponse(response)} />
                  <b>{response.votes}</b>
                  <DownvoteButton postId={response.id} onDownvote={() => handleDownvoteResponse(response)} />
                </div>
              </div>
              <div className='col'>
                <h3>{response.title}</h3>
                  <RenderHTML HTML={response.body} />
                  <div className='row'>
                    <div className='col-1 offset-lg-7 offset-md-3 offset'>
                      <EditResponse RESPONSE={response} />
                    </div>
                    <div className='col-8 offset-md-2 col-lg-3 offset-lg-1 col-md-5 offset-3 bg-secondary text-light p-2 rounded'>
                      <PostUserInfo item={response}/>
                    </div>
                  </div>
                  <br />
                  <PrintComments comments={response.comments}/>
                {isCommentVisibleResponse[response.id] ? (
                  <div>
                    <div className="row">
                      <div className="col-10 d-flex flex-column">
                        <textarea
                          rows={4}
                          value={commentTextResponse}
                          onChange={(e) => setCommentTextResponse(e.target.value)}
                        />
                        <small className="text-muted">Enter at least 15 characters</small>
                      </div>
                      <div className="col d-flex align-items-end my-2">
                        <button
                          className="mx-2 btn btn-secondary"
                          onClick={() => handleCommentSubmitResponse(response.id)}
                          disabled={!isResponseCommentValid()}
                        >
                          Add Comment
                        </button>
                      </div>
                    </div>
                  </div>
                ) : (
                  <div className='container my-2'>
                    <button onClick={() => handleCommentClickResponse(response.id)} className="btn btn-secondary">Add a comment</button>
                  </div>                  
                )
                }
              </div>
            </div>
          </div>
        ))
      ) : (
        <p></p>
      )}
      <div className='container bg-light m-5'>
        <h3>Your Answer</h3>
        <textarea
          rows={6}
          value={responseText}
          onChange={(e) => setResponseText(e.target.value)}
          style={{ width: '100%', resize: 'none' }}
        />
        <button className='btn btn-primary my-4' onClick={() => submitResponse(post?.postItem.id)} disabled={!isResponseValid()}>
          Submit Post
        </button>
      </div>
      
    </div>
  );
};

export default PostDetail;
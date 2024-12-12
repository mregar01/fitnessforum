import React from 'react';
import './App.css'

interface UpvoteButtonProps {
  postId: number | undefined;
  onUpvote: () => void; // Callback function to update the post after upvoting
}

const UpvoteButton: React.FC<UpvoteButtonProps> = ({ postId, onUpvote }) => {

  const handleUpvote = () => {
    if (postId !== undefined) {
      fetch(`/api/posts/${postId}/increment`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      })
        .then(response => {
          if (response.ok) {
            onUpvote(); // Call the callback function to update the post
          } else {
            console.error('Failed to upvote the post');
          }
        })
        .catch(error => {
          console.error('Error occurred while upvoting:', error);
        });
    } else {
      console.error('Invalid postId');
    }
  };

  return (
    <button className='upvote-button my-2' onClick={handleUpvote}>
    </button>
  );
};

export default UpvoteButton;
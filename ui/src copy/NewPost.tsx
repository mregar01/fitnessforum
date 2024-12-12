import { Link } from 'react-router-dom';
import React, { useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import LoadingOverlay from './LoadingOverlay'

const NewPost: React.FC = () => {

  const [titleText, setTitleText] = useState(''); 
  const [bodyText, setBodyText] = useState('');
  const [tagText, setTagText] = useState('');
  const [isPosting, setIsPosting] = useState(false);

  const isFormValid = () => {
    return titleText.trim() !== '' && bodyText.trim() !== '' && tagText.trim() !== '';
  };

  const submitPost = async () => {
    setIsPosting(true);
    try {
      const response = await fetch('/api/posts/new', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          postTitle: titleText,
          postBody: bodyText,
          postTags: tagText,
        }),
        credentials: 'include',
      });
  
      if (response.ok) {
        setTitleText('');
        setBodyText('');
        setTagText('');
        setIsPosting(false);
      } else {
        throw new Error(`HTTP status ${response.status}`);
      }
    } catch (error) {
      console.error(error);
    }
  };

  if (isPosting) {
    return <LoadingOverlay message="Posting..." />;
  }

  return (
    <div className='container'>
      <div className='row'>
        <div className='col-8 offset-1'>
          <h2 className='my-5'>Ask a public question</h2>
          <div className='container bg-light'>
            <h4>Title</h4>
            <small>Be specific and imagine you're asking a question to another person</small><br></br>
            <textarea
              rows={1}
              value={titleText}
              onChange={(e) => setTitleText(e.target.value)}
              style={{ width: '100%', resize: 'none' }}
              placeholder="What's your physical fitness question? Be specific."
            />
            <h4>Body</h4>
            <small>Include all the information someone would need to answer your question</small>
            <textarea
              rows={6}
              value={bodyText}
              onChange={(e) => setBodyText(e.target.value)}
              style={{ width: '100%', resize: 'none' }}
            />
            <h4>Tags</h4>
            <small>Add up to 5 tags to describe what your question is about</small>
            <textarea
              rows={1}
              value={tagText}
              onChange={(e) => setTagText(e.target.value)}
              style={{ width: '100%', resize: 'none' }}
              placeholder="e.g (<squats>, <diet>, <injury-prevention>)"
            />
          </div>
          <button className='btn btn-primary my-4' onClick={() => submitPost()} disabled={!isFormValid()}>
            Submit Post
          </button>
          <Link to="/">
            <button className='btn btn-primary mx-2'>Go to Main Page</button>
          </Link>
        </div>
      </div>      
    </div>
  )
}

export default NewPost;
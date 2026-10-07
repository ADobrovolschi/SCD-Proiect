package com.utcn.ProiectSCD.post;

import com.utcn.ProiectSCD.user.User;
import com.utcn.ProiectSCD.email.EmailService;
import com.utcn.ProiectSCD.user.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class PostService {
    @Autowired
    private PostRepository postRepository;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private EmailService emailService;

    @Transactional
    public Post create(Post myPost, String userEmail) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> new RuntimeException("User not found"));
        myPost.setUser(user);
        Post savedPost = postRepository.save(myPost);

        try {
            emailService.sendPostNotification(savedPost);
        } catch (Exception e) {
            // Log error but don't stop the post creation
            System.err.println("Failed to send email: " + e.getMessage());
        }

        return savedPost;
    }
    // READ ALL
    public List<Post> findAll() {
        return postRepository.findAll();
    }

    // READ ONE
    public Post findById(int id) {
        return postRepository.findById(id).orElse(null);
    }

    // UPDATE
    @Transactional
    public Post update(int id, Post postDetails) {
        Post post = postRepository.findById(id).orElse(null);
        if(post == null) {
            return null;
        }

        post.setTitle(postDetails.getTitle());
        post.setContent(postDetails.getContent());
        post.setStatus(postDetails.getStatus());

        return postRepository.save(post);
    }

    // DELETE
    @Transactional
    public boolean delete(int id) {
        if(!postRepository.existsById(id)) {
            return false;
        }
        postRepository.deleteById(id);
        return true;
    }

    // CUSTOM QUERY
    public List<Post> getAllPublishedPosts() {
        return postRepository.findByStatus(Status.PUBLISHED);
    }

    public List<Post> getAllPendingPosts() {
        return postRepository.findByStatus(Status.PENDING);
    }

    public List<Post> searchPosts(String keyword) {
        return postRepository.searchByKeyword(keyword);
    }
}
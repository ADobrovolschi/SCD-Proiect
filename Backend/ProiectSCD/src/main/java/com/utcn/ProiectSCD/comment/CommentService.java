package com.utcn.ProiectSCD.comment;


import com.utcn.ProiectSCD.user.User;
import com.utcn.ProiectSCD.user.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class CommentService {
    @Autowired
    private CommentRepository commentRepository;

    @Autowired
    private UserRepository userRepository;

    @Transactional
    public Comment create(Comment comment, String userEmail) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> new RuntimeException("User not found"));
        comment.setUser(user);
        return commentRepository.save(comment);
    }

    public List<Comment> findAllComments() {
        return commentRepository.findAll();
    }

    public Comment findById(int id) {
        return commentRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Comment not found with id: " + id));
    }

    public List<Comment> findByPostId(int postId) {
        return commentRepository.findByPostId(postId);
    }

    @Transactional
    public Comment update(int id, Comment updatedComment) {
        Comment existingComment = findById(id);
        existingComment.setContent(updatedComment.getContent());
        return commentRepository.save(existingComment);
    }

    @Transactional
    public void delete(int id) {
        Comment comment = findById(id);
        commentRepository.delete(comment);
    }
}
package com.utcn.ProiectSCD.comment;



import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/comment")
@CrossOrigin
public class CommentController {
    @Autowired
    private CommentService commentService;

    @PostMapping
    public Comment create(@RequestBody Comment comment, Authentication authentication) {
        UserDetails userDetails = (UserDetails) authentication.getPrincipal();
        String userEmail = userDetails.getUsername();
        return commentService.create(comment, userEmail);
    }

    @GetMapping
    public List<Comment> getAllComments() {
        return commentService.findAllComments();
    }

    @GetMapping("/{id}")
    public Comment getById(@PathVariable int id) {
        return commentService.findById(id);
    }

    @GetMapping("/post/{postId}")
    public List<Comment> getByPostId(@PathVariable int postId) {
        return commentService.findByPostId(postId);
    }

    @PutMapping("/{id}")
    public Comment update(@PathVariable int id, @RequestBody Comment comment) {
        return commentService.update(id, comment);
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable int id) {
        commentService.delete(id);
    }
}
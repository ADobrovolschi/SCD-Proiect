package com.utcn.ProiectSCD.admin;

import com.utcn.ProiectSCD.comment.Comment;
import com.utcn.ProiectSCD.comment.CommentRepository;
import com.utcn.ProiectSCD.post.PostRepository;
import com.utcn.ProiectSCD.user.User;
import com.utcn.ProiectSCD.user.UserRepository;
import com.utcn.ProiectSCD.post.Post;
import com.utcn.ProiectSCD.post.PostService;
import com.utcn.ProiectSCD.post.Status;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/admin")
@CrossOrigin
@RequiredArgsConstructor
public class AdminController {

    private final PostService postService;
    private final UserRepository userRepository;
    private final PostRepository postRepository;
    private final CommentRepository commentRepository;

    @GetMapping("/users")
    public ResponseEntity<List<User>> getAllUsers() {
        return ResponseEntity.ok(userRepository.findAll());
    }

    @PutMapping("/posts/{id}/approve")
    public ResponseEntity<Post> approvePost(@PathVariable int id) {
        Post post = postService.findById(id);
        post.setStatus(Status.PUBLISHED);
        return ResponseEntity.ok(postService.update(id, post));
    }

    @PutMapping("/posts/{id}/reject")
    public ResponseEntity<Post> rejectPost(@PathVariable int id) {
        Post post = postService.findById(id);
        post.setStatus(Status.REMOVED);
        return ResponseEntity.ok(postService.update(id, post));
    }

    @DeleteMapping("/users/{id}")
    public ResponseEntity<?> banUser(@PathVariable int id) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("User not found"));

        // Mai întâi ștergem sau actualizăm postările
        List<Post> userPosts = postRepository.findByUserId(id);
        for (Post post : userPosts) {
            post.setStatus(Status.REMOVED);
            post.setUser(null);
            postRepository.save(post);
        }

        // Apoi ștergem sau actualizăm comentariile
        List<Comment> userComments = commentRepository.findByUserId(id);
        for (Comment comment : userComments) {
            comment.setUser(null);
            commentRepository.save(comment);
        }

        // În final ștergem utilizatorul
        userRepository.delete(user);
        return ResponseEntity.ok().build();
    }
}
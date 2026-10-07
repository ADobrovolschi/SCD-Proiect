package com.utcn.ProiectSCD.post;

import org.springframework.security.core.Authentication;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/post")
@CrossOrigin
public class PostController {
    @Autowired
    private PostService postService;

    @PostMapping
    public Post create(@RequestBody Post myPost, Authentication authentication) {
        UserDetails userDetails = (UserDetails) authentication.getPrincipal();
        String userEmail = userDetails.getUsername();
        return postService.create(myPost, userEmail);
    }

    // READ ALL
    @GetMapping
    public List<Post> getAllPosts() {
        return postService.findAll();
    }

    // READ ONE
    @GetMapping("/{id}")
    public ResponseEntity<Post> getPostById(@PathVariable int id) {
        Post post = postService.findById(id);
        if(post == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(post);
    }

    // UPDATE
    @PutMapping("/{id}")
    public ResponseEntity<Post> updatePost(@PathVariable int id, @RequestBody Post postDetails) {
        Post updatedPost = postService.update(id, postDetails);
        if(updatedPost == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(updatedPost);
    }

    // DELETE
    @DeleteMapping("/{id}")
    public ResponseEntity<?> deletePost(@PathVariable int id) {
        boolean deleted = postService.delete(id);
        if(!deleted) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok().build();
    }

    // CUSTOM QUERY - Get all published posts
    @GetMapping("/published")
    public List<Post> getAllPublishedPosts() {
        return postService.getAllPublishedPosts();
    }
    @GetMapping("/pending")
    public List<Post> getAllPendingPosts() {
        return postService.getAllPendingPosts();
    }
    @GetMapping("/search")
    public List<Post> searchPosts(@RequestParam String keyword) {
        return postService.searchPosts(keyword);
    }
}